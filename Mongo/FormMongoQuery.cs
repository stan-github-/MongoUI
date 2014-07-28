using System;
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
        private QueryExecuter _queryExecuter;

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
            this.text_box.KeyUp += text_box_KeyUp;

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
            _queryExecuter = new QueryExecuter(this);

            return true;
        }

        
        #region "autocomplete"

        void text_box_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.OemPeriod)
            {
                return;
            }

            //this.text_box.AutoComplete.List = new List<String>() { "asf", "badf", "sdfc" };
            //this.text_box.AutoComplete.Show(0, new List<String>() { "asf", "badf", "sdfc" });

            //var methodName = MethodFinder.Run(this.text_box);
        }

        //dot after a name, dot after a close parenthesis
        //build collection of method name, mongo and underscore
        //build collection of db collections

        public class MethodFinder {

            public static String Run(ScintillaNET.Scintilla text_box) {

                var s = text_box.Text.Substring(0, text_box.CurrentPos - 1);
                var brackets = MethodFinder.GetQueryDelimiters(s);
                int itemsTaken;
                bool matchFound = false;

                var bracketsToRemove = RemoveMatchingBrackets(brackets, out itemsTaken, out matchFound).OrderBy(x => x.Index);
                
                String methodName = String.Empty;
                if (matchFound == true)
                {
                    methodName = GetMethodName(bracketsToRemove.First(), s);
                }

                foreach (var b in bracketsToRemove.OrderBy(x => x.Index))
                {
                    ErrorManager.Write(b.Index.ToString());
                    ErrorManager.Write(b.Groups[0].ToString());
                }

                ErrorManager.Write(matchFound ? "match found" : "not matching delimiters");
                ErrorManager.Write(methodName);
                
                return methodName;
            }

            private static String GetMethodName(Match firstBracket, String s)
            {
                var word = new List<char>();
                var chars = s.ToArray<char>();

                for (int i = firstBracket.Groups[0].Index - 1; i > -1; i--)
                {
                    var c = chars[i];
                    if (Char.IsLetter(c) || Char.IsNumber(c) || Char.IsWhiteSpace(c))
                    {
                        word.Add(c);
                    }
                    else
                    {
                        break;
                    }
                }

                var Word = new StringBuilder();
                word.Reverse();
                word.ForEach(x => Word.Append(x.ToString()));

                return Word.ToString().Trim();
            }
            
            //delimiters = quotes + brackets
            private static List<Match> GetQueryDelimiters(String s)
            {
                string regex = @"\{|\}|\(|\)|\'|\""|\[|\]";
                var options = RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Singleline;
                var reg = new Regex(regex, options);
                try
                {
                    var delimiters = reg.Split(s);

                    var c = reg.Matches(s);
                    var list = new List<Match>();

                    int i = 0;

                    foreach (Match q in c)
                    {
                        list.Add(q);
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    ErrorManager.Write(ex);
                }

                return new List<Match>();
            }

            private static List<Match> RemoveMatchingQuotes(List<Match> delimiters, out int itemsTaken, out bool matchFound)
            {
                var quotes = new List<String> { @"""", "'" };
                if (delimiters.Count == 0 ||
                    !quotes.Contains(delimiters.Last().Groups[0].ToString()))
                {
                    itemsTaken = 0;
                    matchFound = true;
                    return delimiters;
                }

                int length = delimiters.Count;
                var toRemove = new List<Match>();

                Match closeDelimiter = null;
                Match delimiter = null;

                String closeDelimiterString = String.Empty;
                String delimiterString = String.Empty;

                for (int i = length - 1; i > -1; i--)
                {
                    delimiter = delimiters[i];
                    delimiterString = delimiter.Groups[0].ToString();

                    //first delimiter,
                    if (i == length - 1)
                    {
                        closeDelimiter = delimiter;
                        closeDelimiterString = closeDelimiter.Groups[0].ToString();
                        toRemove.Add(closeDelimiter);
                        continue;
                    }

                    //if close quote, return items
                    if (delimiterString == closeDelimiterString)
                    {
                        toRemove.Add(delimiter);
                        itemsTaken = toRemove.Count;
                        matchFound = true;
                        return toRemove;
                    }
                    else
                    {
                        toRemove.Add(delimiter);
                        continue;
                    }
                }

                itemsTaken = length;
                matchFound = false;
                return toRemove;
            }

            //recursively remove brackets
            private static List<Match> RemoveMatchingBrackets(List<Match> delimiters, out int itemsTaken, out bool matchFound)
            {
                int length = delimiters.Count;
                var bracketDict = new Dictionary<String, String>()
                {
                    {")", "("},  {@"}", @"{"}, {@"]", @"["}
                };
                var quotes = new List<String> { @"""", "'" };

                var closeBrackets = new List<String> { @"}", @")", @"]" };
                var toRemove = new List<Match>();

                Match closeDelimiter = null;
                Match delimiter = null;

                String closeDelimiterString = String.Empty;
                String delimiterString = String.Empty;

                for (int i = length - 1; i > -1; i--)
                {
                    delimiter = delimiters[i];
                    delimiterString = delimiter.Groups[0].ToString();
                    //first bracket, assume it's a close bracket
                    if (i == length - 1)
                    {
                        closeDelimiter = delimiter;
                        closeDelimiterString = closeDelimiter.Groups[0].ToString();
                        toRemove.Add(delimiter);
                        continue;
                    }

                    //if open bracket, return items
                    if (bracketDict.Keys.Contains(closeDelimiterString) &&
                        bracketDict[closeDelimiterString] == delimiterString)
                    {
                        toRemove.Add(delimiter);
                        itemsTaken = toRemove.Count;
                        matchFound = true;
                        return toRemove;
                    }

                    //if hits quotes, call remvoing matching quotes
                    if (quotes.Contains(delimiterString))
                    {
                        int itemsToSkip;
                        toRemove.AddRange(
                            RemoveMatchingQuotes(delimiters.Take(i + 1).ToList(), out itemsToSkip,
                            out matchFound));
                        i = i - itemsToSkip + 1;
                        continue;
                    }

                    //if hits close bracket, remove matching bracket
                    if (closeBrackets.Contains(delimiterString))
                    {
                        int itemsToSkip;
                        toRemove.AddRange(
                            RemoveMatchingBrackets(delimiters.Take(i + 1).ToList(), out itemsToSkip, out matchFound));
                        i = i - itemsToSkip + 1;
                        continue;
                    }
                    else
                    {
                        toRemove.Add(delimiter);
                        continue;
                    }
                }

                matchFound = false;
                itemsTaken = length;
                return toRemove;
            }

        }
        #endregion


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
            _queryExecuter.Execute(query);

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
                if (String.IsNullOrWhiteSpace(query))
                {
                    return;
                }

                if (ContinueWithExecutionAfterWarning() == false)
                {
                    return;
                }

                _queryOutput = new StringBuilder();

                var queries = SplitQueries(query);
             
                foreach(Tuple<QueryType, String> q in queries){
                    _queryOutput.Append("--------------------")
                        .Append(q.Item1.ToString())
                        .Append("---------------------")
                        .Append(Environment.NewLine);

                    if (q.Item1 == QueryType.SQL || q.Item1 == QueryType.DOS)
                    {
                        ExecuteExternalQueries(q.Item1, new List<String> { q.Item2 });
                    }
                    else {
                        ExecuteMongo(q.Item2);
                    }
                }

                DispalyQueryOutput(_queryOutput.ToString());
            }
            public QueryExecuter(FormMongoQuery form)
            {
                _form = form;
            }

            //first split query into individual components
            //make sure which is which
            //run through the queries.
            
            public List<Tuple<QueryType, String>> SplitQueries(String query)
            {
                string regex = @"\#\#DOS\#\#|\#\#MONGO\#\#|\#\#SQL\#\#";
                var options = RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Singleline;

                var tokens = new List<String> { "##DOS##", "##MONGO##", "##SQL##" };
                var dict = new List<Tuple<QueryType, String>>();

                var reg = new Regex(regex, options);
                var s = query.Trim();

                try
                {
                    var queries = reg.Split(s);
                    if (queries == null || queries.Length == 0)
                    {
                        return null;
                    }

                    if (queries.Length == 1)
                    {
                        return new List<Tuple<QueryType, String>>{ 
                          new Tuple<QueryType,String>(QueryType.MONGO, queries[0])};
                    }

                    var c = reg.Matches(s);

                    int i = 0;
                    foreach (Match q in c)
                    {
                        //if the first query has no token assume it's mongo
                        if (i == 0)
                        {
                            if (q.Index != 0)
                            {
                                dict.Add(new Tuple<QueryType, String>
                                    (QueryType.MONGO, queries[0]));
                            }
                        }

                        QueryType queryType;
                        var queryType_ = q.Groups[0].ToString().Replace("#", "");
                        if (!Enum.TryParse<QueryType>(queryType_, out queryType)){
                            throw new Exception(queryType_ + " is not valid query type");
                        }

                        dict.Add(new Tuple<QueryType, String>(
                            queryType, queries[i + 1]));
                        i++;
                    }
                }
                catch (Exception ex) {
                    ErrorManager.Write(ex);
                }
                return dict;
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

                if (type == QueryType.DOS)
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
                MONGO,
                SQL,
                DOS
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
                //query = StripExternalQueries(query);

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
