using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DBUI.Queries
{
    //todo find the line number of the error, and high light the test in the window!!
    public class QueryHelper {
        //need to get rid of _form variable, just pass in strings...
        //private FormMongoQuery _form;

        public String TempJSFile { get; set; }

        #region "query output" 

        public String QueryOutputAll {get {
            return String.Format("{0}\n\r{1}\n\r",
                StandardError.ToString(),
                StandardOut.ToString());
            } 
        }
        public String JavascriptQueryError
        {
            /*
             * 
             * Sun Mar 29 21:27:37.660 JavaScript execution failed: ReferenceError: 
            zzz is not defined at 
            E:\Users\ztan\AppData\Local\Temp\\69d3425f-150d-4d1d-abab-1be699fabac0.js:L26
            failed to load: E:\Users\ztan\AppData\Local\Temp\\69d3425f-150d-4d1d-abab-1be699fabac0.js
             */
            get
            {
                if (StandardOut.ToString().Contains("JavaScript execution failed:")
                    //&& standardOut.Contains("ReferenceError:")
                )
                {
                    var array = this.StandardOut.ToString()
                        .Split(new string[] { "failed to load:", this.TempJSFile }, 
                         StringSplitOptions.RemoveEmptyEntries);

                    if (array.Length < 3 || !array[1].Contains(":L")) {
                        throw new Exception("javascript error message parse error!!, please update mongo and mongo ui!");
                    }
                    
                    var lineNumber = array[1].Replace(":L", "").Replace("\n\r", "");

                    int lineNumberInt = 0;

                    if (!int.TryParse(lineNumber, out lineNumberInt)) {
                        throw new Exception("javascript error message parse error!!, please update mongo and mongo ui!");
                    }

                    var sb = new StringBuilder();
                    sb.Append(array[0]).Append(Environment.NewLine);
                    sb.Append("line number: ")
                      .Append((lineNumberInt - this.QueryErrorLineNumOffset).ToString());

                    return sb.ToString();
                    //return
                    //(String.Format("{0}\n\rcustom code lines: {1}",
                    //    this.StandardOut,
                    //    QueryErrorLineNumOffset));

                }
                return String.Empty;
            }
        }
        public StringBuilder StandardOut {get; set;}
        public StringBuilder StandardError { get; set; }
        
        public int QueryErrorLineNum {
            get { 
                if (String.IsNullOrEmpty(this.JavascriptQueryError.ToString())){
                    return 0;
                }
                return 0;
            }
        }

        public int QueryErrorLineNumOffset { get; set; }
        #endregion

        #region "auto execute, no feedback"
        
        public bool NoWindows { get; set; }
        
        //overrides server configuration
        //for querying collection names
        public bool NoConfirmation { get; set; }
        
        public bool NoFeedBack
        {
            get { return NoWindows && NoConfirmation; }
            set { NoWindows = true; NoConfirmation = true;  }
        }

        public bool ContinueWithExecutionAfterWarning()
        {
            if (NoConfirmation)
            {
                return true;
            }

            var serverName = MongoXMLManager.CurrentServer.Name;

            if (!MongoXMLManager.Servers.First(s => s.Name == serverName).WithWarning)
            {
                return true;
            }

            if (MessageBox.Show("Continue query with " + serverName, "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region "actual execution"

        public String PrepareJsFile(String query)
        {
            //apppend custom code to file
            var customJsCode = PrependCustomJSCode();
            FileManager.SaveToFile(this.TempJSFile, customJsCode);
            FileManager.AppendToFile(this.TempJSFile, query);

            //set line offset for query feed back
            QueryErrorLineNumOffset = customJsCode.Split('\n').Length;

            return this.TempJSFile;
        }

        private String PrependCustomJSCode()
        {
            var b = new StringBuilder();
            foreach (var path in MongoXMLManager.CustomJSFilePaths)
            {
                b.Append(FileManager.ReadFromFile(path)).Append("\n");
            }

            return b.ToString();
        }

        private void SetTempFilePaths()
        {
            TempJSFile = Environment.ExpandEnvironmentVariables(MongoXMLManager.TempFolderPath
                                                              + "\\" + Guid.NewGuid() + ".js");
        }

        #endregion

        MongoXMLRepository MongoXMLManager { get; set; }

        //todo, not the best implementation, code smell
        public void Init() {
            //this.JavascriptError = new StringBuilder();
            this.StandardError = new StringBuilder();
            this.StandardOut = new StringBuilder();

            MongoXMLManager = Program.MongoXMLManager;

            SetTempFilePaths();
        }

        public QueryHelper()
        {
            Init();
        }
        
    }

    public class QueryExecuter
    {
        public MongoXMLRepository MongoXMLManager {get; set; }
        
        QueryHelper _queryHelper;
        public QueryHelper QueryHelper {
            get
            {
                if (_queryHelper == null) {
                    _queryHelper = new QueryHelper();
                    return _queryHelper;
                }
                return _queryHelper;
            }
            set {
                _queryHelper = value;
            }
        }

        private void Init() {
            MongoXMLManager = Program.MongoXMLManager;
        }

        public QueryExecuter()
        {
            Init();
        }

        public String Execute(string query)
        {
            //reset query helper
            QueryHelper.Init();

            //check for query
            if (String.IsNullOrWhiteSpace(query))
            {
                return String.Empty;
            }

            //warning check (running again production for example)
            if (QueryHelper.ContinueWithExecutionAfterWarning() == false)
            {
                return String.Empty;
            }

            //prepend custome js files etc and return file path
            var tempFilePath = QueryHelper.PrepareJsFile(query);

            //actually executing the query using file
            ExecuteMongo(tempFilePath);

            //delete file
            FileManager.DeleteFile(tempFilePath);

            //return output
            return QueryHelper.QueryOutputAll.ToString();
        }
           
        private void ExecuteMongo(String tempFilePath)
        {
            
            //execute file
            String arguments = String.Format(
                "{0} --quiet --host {1} {2} ",
                MongoXMLManager.CurrentServer.CurrentDatabase.Name,
                MongoXMLManager.CurrentServer.Name,
                tempFilePath);

            ExecuteConsoleApp("mongo.exe", arguments);

        }

        private void ExecuteConsoleApp(String exeName, String arguments)
        {
            Process process = new Process();
            process.StartInfo.FileName = exeName; //"mongo.exe ";

            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = QueryHelper.NoWindows;
            process.Start();

            //standard error, mongo.exe not found etc
            QueryHelper.StandardError.Append(process.StandardError.ReadToEnd());

            //standard out
            QueryHelper.StandardOut.Append(process.StandardOutput.ReadToEnd());

            process.WaitForExit();

        }

        //todo need cleanup refactoring etc
        //display output in notepad, excel etc.
        public void DisplayQueryInExe(String content, String exe)
        {
            string tempPath = Environment.ExpandEnvironmentVariables
                (MongoXMLManager.TempFolderPath + "\\"
                 + Guid.NewGuid() + ".json");
            ;
            if (FileManager.SaveToFile(tempPath, content)
                == false)
            {
                return;
            }

            Process process = new Process();
            process.StartInfo.FileName = exe;
            process.StartInfo.Arguments = tempPath;
            process.Start();
        }

        #region additional types of execution dos, sql etc
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
        private String Execute2(string query)
        {
            //reset query helper
            QueryHelper.Init();

            if (String.IsNullOrWhiteSpace(query))
            {
                return String.Empty;
            }

            if (QueryHelper.ContinueWithExecutionAfterWarning() == false)
            {
                return String.Empty;
            }

            var queries = SplitQueries(query);

            //foreach (Tuple<QueryType, String> q in queries)
            //{
            //    QueryHelper.SetOutputPrefix(q.Item1.ToString());

            //    if (q.Item1 == QueryType.SQL || q.Item1 == QueryType.DOS)
            //    {
            //        ExecuteExternalQueries(q.Item1, new List<String> { q.Item2 });
            //    }
            //    else
            //    {
            //        ExecuteMongo(q.Item2);
            //    }
            //}

            return QueryHelper.QueryOutputAll.ToString();
        }

        private void ExecuteDOSCmdLine(String command)
        {

            if (string.IsNullOrWhiteSpace(command))
            {
                return;
            }

            var tempFile = "";// QueryHelper.TempBatFile;
            FileManager.SaveToFile(tempFile, "");
            FileManager.AppendToFile(tempFile, command);

            ExecuteConsoleApp(tempFile, String.Empty);
            FileManager.DeleteFile(tempFile);
        }

        private void ExecuteExternalQueries(QueryType type, List<String> queries)
        {
            String arguments = string.Empty;
            String fileName = string.Empty;

            var sqlCmd = MongoXMLManager.SQlCmd;

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
                    ExecuteDOSCmdLine(q);
                }
            }
        }

        public enum QueryType
        {
            MONGO,
            SQL,
            DOS
        }
        
        #endregion

    }

}
