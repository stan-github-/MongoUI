﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DBUI;
using System.Diagnostics;

namespace DBUI.Mongo {
    public partial class FormMongoQuery : Form
    {
        private QueryExecuter _executeQuery;

        public enum Mode
        {
            New,
            FileDialog,
            Existing,
            Last
        }

        public String Title {
            get { return this.text_box.Text; }
        }

        public String QueryFilePath { get; set; }

        public String TempJSFile
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(
                    Program.MongoXMLManager.TempFolderPath
                    + "\\" + Guid.NewGuid() + ".js");
            }
        }

        public Mode mode { get; set; }

        public FormMongoQuery(FormMainMDI parent)
        {
            InitializeComponent();
            this.MdiParent = parent;
            splitContainer1.Panel2Collapsed = true;
            splitContainer1.Panel2.Hide();
        }

        public bool Init(Mode mode, String filePath = null)
        {
            this.text_box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownHandler);

            //mode whether new or existing
            switch (mode)
            {
                case Mode.New:
                    EnsureQueryFilePathExists();
                    break;
                case Mode.Existing:
                    this.QueryFilePath = filePath;
                    EnsureQueryFilePathExists();
                    break;
                case Mode.Last:
                    this.QueryFilePath = Program.MongoXMLManager.LastFilePath;
                    EnsureQueryFilePathExists();
                    break;
                case Mode.FileDialog:
                    this.QueryFilePath = this.OpenOpenFileDialog();
                    EnsureQueryFilePathExists();
                    break;
            }


            //form tile
            this.Text = this.QueryFilePath;
            this.text_box.Text = FileManager.ReadFromFile(this.QueryFilePath);

            //resize window
            this.WindowState = FormWindowState.Maximized;
            this.Show();

            //set output type
            SetQueryOutputDisplayType();

            //instantiate child class
            _executeQuery = new QueryExecuter(this);

            return true;
        }

        #region "control init"
        private void SetQueryOutputDisplayType()
        {
            foreach (var t in Program.MongoXMLManager.QueryOutputTypes.Types)
            {
                this.OutputTypeComboBox.Items.Add(t);
            }

            this.OutputTypeComboBox.Text = Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType;
        }

        #endregion

        #region "control event handlers"

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                ExecuteQueryAndSaveToFile();
            }
        }

        private void button_excecute_Click(object sender, EventArgs e)
        {
            ExecuteQueryAndSaveToFile();
        }

        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            Program.MongoXMLManager.SaveXml();
        }

        private void QueryOutputType_Selected(object sender, EventArgs e)
        {
            Program.MongoXMLManager.QueryOutputTypes =
                new MongoXMLRepository.QueryOutputType() { CurrentOutputType = OutputTypeComboBox.Text };
        }

        #endregion

        #region "file/query handlers"
        private void ExecuteQueryAndSaveToFile()
        {
            var query = String.IsNullOrEmpty(text_box.Selection.Text)
                                ? this.text_box.Text
                                : text_box.Selection.Text;
            _executeQuery.Execute(query);

            //save file
            FileManager.SaveToFile(this.QueryFilePath, text_box.Text);
        }

        private void EnsureQueryFilePathExists()
        {
            if (!File.Exists(QueryFilePath))
            {
                QueryFilePath = TempJSFile;
                FileManager.SaveToFile(QueryFilePath, "//new query");
            }
        }
        
        private string OpenOpenFileDialog()
        {
            this.open_file_dialog.InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            this.open_file_dialog.Filter = "JS Files (*.js)|*.js|All Files (*.*)|*.*";
            
            //minimize window, can't hide
            if (this.MdiParent != null && this.MdiParent.ActiveMdiChild != null)
            {
                this.open_file_dialog.InitialDirectory = 
                    Path.GetDirectoryName(this.MdiParent.ActiveMdiChild.Text);
            }
            this.WindowState = FormWindowState.Minimized;
            if (this.open_file_dialog.ShowDialog(this) != DialogResult.OK)
            {
                return String.Empty;
            }
            return this.open_file_dialog.FileName;
        }
        #endregion

        /// <summary>
        /// Query (Mongo, SQL) Execution Handler
        /// </summary>
        
        //todo
        //mongo/sql/dos cmds should be executed in sequence, 
        //additionally should just split the queryies with special tokens
        public class QueryExecuter
        {
            //need to get rid of _form variable, just pass in strings...
            private FormMongoQuery _form;
            private StringBuilder _queryOutput;
            private String tempJSFile
            {
                get
                {
                    return Environment.ExpandEnvironmentVariables(Program.MongoXMLManager.TempFolderPath
                                                                  + "\\" + Guid.NewGuid() + ".js");
                }
            }

            private String tempBatFile
            {
                get
                {
                    return Environment.ExpandEnvironmentVariables(Program.MongoXMLManager.TempFolderPath
                                                                  + "\\" + Guid.NewGuid() + ".bat");
                }
            }

            public void Execute(string query)
            {
                if (String.IsNullOrEmpty(query))
                {
                    return;
                }

                if (ContinueWithExecutionAfterWarning() == false)
                {
                    return;
                }

                _queryOutput = new StringBuilder();

             
                //could use a loop to loop through the query types
                var sqlQueries = GetExternalQueryResults(QueryType.SQL, query);
                ExecuteExternalQueries(QueryType.SQL, sqlQueries);

                var cmdlines = GetExternalQueryResults(QueryType.CmdLine, query);
                ExecuteExternalQueries(QueryType.CmdLine, cmdlines);

                ExecuteMongo(query);

                //display query output;
                //todo could be updated to display error line with mongo query
                DispalyQueryOutput(_queryOutput.ToString());
            }
            public QueryExecuter(FormMongoQuery form)
            {
                _form = form;
            }

            private bool ContinueWithExecutionAfterWarning()
            {
                var serverName = ((FormMainMDI)_form.ParentForm).ServerName;

                if (!Program.MongoXMLManager.Servers.First(s => s.Name == serverName).WithWarning)
                {
                    return true;
                }

                if (MessageBox.Show("Continue query with " + serverName, "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    return true;
                }

                return false;
            }

            

            private List<String> GetExternalQueryMatches(QueryType type, String query)
            {
                var reg = GetExternalQueryRegex(type);
                var queries = new List<String>();

                foreach (Match q in reg.Matches(query))
                {
                    queries.Add(q.ToString());
                }
                return queries;
            }

            private List<String> GetExternalQueryResults(QueryType type, String query)
            {
                var reg = GetExternalQueryRegex(type);
                var queries = new List<String>();

                foreach (Match q in reg.Matches(query))
                {
                    queries.Add(q.Result("$1").Replace(Environment.NewLine, " "));
                }
                return queries;
            }

            private void ExecuteExternalQueries(QueryType type, List<String> queries)
            {
                String arguments = string.Empty;
                String fileName = string.Empty;

                var sqlCmd = Program.MongoXMLManager.SQlCmd;

                if (type == QueryType.SQL)
                {
                    foreach (var q in queries)
                    {
                        arguments = String.Format("-S {0} -d {1} -q \"{2}\"",
                            sqlCmd.Server, sqlCmd.Database, q);
                        ExecuteConsoleApp(sqlCmd.ExePath, arguments);
                    }
                }

                if (type == QueryType.CmdLine)
                {
                    foreach (var q in queries)
                    {
                        //arguments = "/c " + q;
                        //ExecuteConsoleApp("cmd.exe", arguments);
                        ExecuteCmdLine(q);
                    }
                }
            }

            public enum QueryType
            {
                Mongo,
                SQL,
                CmdLine
            }

            private Regex GetExternalQueryRegex(QueryType type)
            {
                string regex = @"\b" + type.ToString() + @"\b[<]{2}(.*?)[>]{2}";
                var options = RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Singleline;
                var reg = new Regex(regex, options);
                return reg;
            }

            private String StripExternalQueries(String query)
            {
                String q = query;
                foreach (var t in new List<QueryType> { QueryType.SQL, QueryType.CmdLine })
                {
                    foreach (var s in GetExternalQueryMatches(t, query))
                    {
                        q = q.Replace(s, "");
                    }
                }
                return q;
            }

            private void DisplayErrorCode(String tempFile, ref String outputMessage)
            {
                //use regex to get error line.
                //read tempFile to get the code.
                //append text to outputmessage.
            }

            private void ExecuteConsoleApp(String exeName, String arguments)
            {
                Process process = new Process();
                process.StartInfo.FileName = exeName; //"mongo.exe ";

                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();

                _queryOutput.Append(process.StandardOutput.ReadToEnd());
                _queryOutput.Append(Environment.NewLine);
                _queryOutput.Append(process.StandardError.ReadToEnd());
                process.WaitForExit();

            }

            private void ExecuteCmdLine(String command)
            {
                //mongo.exe must be in path variable, 
                //mongod must be started as service or console app

                if (string.IsNullOrWhiteSpace(command))
                {
                    return;
                }

                var tempFile = tempBatFile;
                FileManager.SaveToFile(tempFile, "");
                FileManager.AppendToFile(tempFile, command);

                ExecuteConsoleApp(tempFile, String.Empty);
                FileManager.DeleteFile(tempFile);
            }


            private void ExecuteMongo(String query)
            {
                //mongo.exe must be in path variable, 
                //mongod must be started as service or console app
                query = StripExternalQueries(query);

                if (string.IsNullOrWhiteSpace(query))
                {
                    return;
                }

                //apppend custom code to file
                var tempFile = tempJSFile;
                FileManager.SaveToFile(tempFile, PrependCustomJSCode(""));
                FileManager.AppendToFile(tempFile, query);

                //execute file
                String arguments = String.Format(
                    "{0} --host {1} {2} ",
                    ((FormMainMDI)_form.ParentForm).DatabaeName,
                    ((FormMainMDI)_form.ParentForm).ServerName,
                    tempFile);

                ExecuteConsoleApp("mongo.exe", arguments);

                FileManager.DeleteFile(tempFile);
            }

            private void DispalyQueryOutput(String content)
            {
                if (!Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType.Contains("MongoUI"))
                {
                    _form.splitContainer1.Panel2Collapsed = true;
                    _form.splitContainer1.Panel2.Visible = false;
                    DisplayQueryInExe(content, Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType);
                }
                else if (Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType == "MongoUI")
                {
                    _form.splitContainer1.Panel2Collapsed = false;
                    _form.splitContainer1.Panel2.Show();
                    _form.scintillaOutput.Text = content;
                }
            }

            private String PrependCustomJSCode(String script)
            {
                var b = new StringBuilder();
                foreach (var path in Program.MongoXMLManager.CustomJSFilePaths)
                {
                    b.Append(FileManager.ReadFromFile(path)).Append("\n");
                }

                return b.Append(script).ToString();
            }

            private void DisplayQueryInExe(String content, String exe)
            {
                string tempPath = Environment.ExpandEnvironmentVariables
                    (Program.MongoXMLManager.TempFolderPath + "\\"
                     + Guid.NewGuid() + ".json");
                ;
                if (FileManager.SaveToFile(tempPath, content)
                    == false)
                {
                    return;
                }

                Process compiler = new Process();
                compiler.StartInfo.FileName = exe;
                compiler.StartInfo.Arguments = tempPath;
                compiler.Start();
            }
        }

    }
}
