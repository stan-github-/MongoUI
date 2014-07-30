using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DBUI.Mongo
{
    public class QueryExecuter
    {
        //need to get rid of _form variable, just pass in strings...
        //private FormMongoQuery _form;
        private StringBuilder _queryOutput;

        public bool NoWindows { get; set; }

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

        public String Execute(string query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                return String.Empty;
            }

            if (ContinueWithExecutionAfterWarning() == false)
            {
                return String.Empty;
            }

            _queryOutput = new StringBuilder();

            var queries = SplitQueries(query);

            foreach (Tuple<QueryType, String> q in queries)
            {
                _queryOutput.Append("--------------------")
                    .Append(q.Item1.ToString())
                    .Append("---------------------")
                    .Append(Environment.NewLine);

                if (q.Item1 == QueryType.SQL || q.Item1 == QueryType.DOS)
                {
                    ExecuteExternalQueries(q.Item1, new List<String> { q.Item2 });
                }
                else
                {
                    ExecuteMongo(q.Item2);
                }
            }

            return _queryOutput.ToString();
            //DispalyQueryOutput(_queryOutput.ToString());
        }
        public QueryExecuter()
        {
            //_form = form;
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
                    if (!Enum.TryParse<QueryType>(queryType_, out queryType))
                    {
                        throw new Exception(queryType_ + " is not valid query type");
                    }

                    dict.Add(new Tuple<QueryType, String>(
                        queryType, queries[i + 1]));
                    i++;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.Write(ex);
            }
            return dict;
        }

        private bool ContinueWithExecutionAfterWarning()
        {
            var serverName = Program.MongoXMLManager.CurrentServer.Name;

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
            process.StartInfo.CreateNoWindow = NoWindows;
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
                Program.MongoXMLManager.CurrentServer.CurrentDatabase.Name,
                Program.MongoXMLManager.CurrentServer.Name,
                //((FormMainMDI)_form.ParentForm).DatabaeName,
                //((FormMainMDI)_form.ParentForm).ServerName,
                tempFile);

            ExecuteConsoleApp("mongo.exe", arguments);

            FileManager.DeleteFile(tempFile);
        }

        //private void DispalyQueryOutput(String content)
        //{
        //    if (!Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType.Contains("MongoUI"))
        //    {
        //        _form.splitContainer1.Panel2Collapsed = true;
        //        _form.splitContainer1.Panel2.Visible = false;
        //        DisplayQueryInExe(content, Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType);
        //    }
        //    else if (Program.MongoXMLManager.QueryOutputTypes.CurrentOutputType == "MongoUI")
        //    {
        //        _form.splitContainer1.Panel2Collapsed = false;
        //        _form.splitContainer1.Panel2.Show();
        //        _form.scintillaOutput.Text = content;
        //    }
        //}

        private String PrependCustomJSCode(String script)
        {
            var b = new StringBuilder();
            foreach (var path in Program.MongoXMLManager.CustomJSFilePaths)
            {
                b.Append(FileManager.ReadFromFile(path)).Append("\n");
            }

            return b.Append(script).ToString();
        }

        public static void DisplayQueryInExe(String content, String exe)
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
