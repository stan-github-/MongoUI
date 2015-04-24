using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBUI.Queries
{
    //todo find the line number of the error, and high light the test in the window!!
    public class MongoQueryHelper {
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
                      .Append((lineNumberInt - this.QueryErrorLineNumOffset + 1).ToString());

                    return sb.ToString();
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

        public String PrepareHtmlFile(String text)
        {
            FileManager.SaveToFile(this.TempJSFile, text);
            return this.TempJSFile;
        }

        private String PrependCustomJSCode()
        {
            var b = new StringBuilder();
            foreach (var path in Program.MongoXMLManager.CustomJSFilePaths)
            {
                b.Append(FileManager.ReadFromFile(path)).Append("\n");
            }

            return b.ToString();
        }

        private void SetTempFilePaths()
        {
            TempJSFile = Environment.ExpandEnvironmentVariables(Program.MainXMLManager.TempFolderPath
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

        public MongoQueryHelper()
        {
            Init();
        }
        
    }

    public class PhantomJsHelper
    {
        //need to get rid of _form variable, just pass in strings...
        //private FormMongoQuery _form;

        public String TempJSFile { get; set; }
        public String TempHTMLFile { get; set; }

        #region "query output"

        public String QueryOutputAll
        {
            get
            {
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
                //if (StandardOut.ToString().Contains("JavaScript execution failed:")
                //    //&& standardOut.Contains("ReferenceError:")
                //)
                //{
                //    var array = this.StandardOut.ToString()
                //        .Split(new string[] { "failed to load:", this.TempJSFile },
                //         StringSplitOptions.RemoveEmptyEntries);

                //    if (array.Length < 3 || !array[1].Contains(":L"))
                //    {
                //        throw new Exception("javascript error message parse error!!, please update mongo and mongo ui!");
                //    }

                //    var lineNumber = array[1].Replace(":L", "").Replace("\n\r", "");

                //    int lineNumberInt = 0;

                //    if (!int.TryParse(lineNumber, out lineNumberInt))
                //    {
                //        throw new Exception("javascript error message parse error!!, please update mongo and mongo ui!");
                //    }

                //    var sb = new StringBuilder();
                //    sb.Append(array[0]).Append(Environment.NewLine);
                //    sb.Append("line number: ")
                //      .Append((lineNumberInt - this.QueryErrorLineNumOffset + 1).ToString());

                //    return sb.ToString();
                //}
                return String.Empty;
            }
        }
        public StringBuilder StandardOut { get; set; }
        public StringBuilder StandardError { get; set; }

        #endregion

        public bool NoWindows { get; set; }

        #region "actual execution"

        public String PrepareJsFile()
        {
            //apppend custom code to file

            var customJsCode = PrependCustomJSCode();
            var jsContent = ReadWrapperJs();

            FileManager.SaveToFile(this.TempJSFile, customJsCode);
            FileManager.AppendToFile(this.TempJSFile, jsContent);

            return this.TempJSFile;
        }

        private String ReadWrapperJs()
        {
            String wrapperJsPath = Application.StartupPath + "/Queries/WrapperJs.js";
            var content = File.ReadAllText(wrapperJsPath);
            content = content.Replace("{replace_replace_replace}", "file:///" + this.TempHTMLFile.Replace(@"\", @"/"));
            return content;
        }

        public String PrepareHtmlFile(String text)
        {
            FileManager.SaveToFile(this.TempHTMLFile, text);
            //copy angular js etc to temp file folder to be used by html file
            PhantomJsXMLRepository.CustomJSFilePaths.ForEach(f => 
                FileManager.CopyFileToFolder(Application.StartupPath + @"\" + f,
                Environment.ExpandEnvironmentVariables(Program.MainXMLManager.TempFolderPath)));
            
            return this.TempHTMLFile;
        }

        private String PrependCustomJSCode()
        {
            var b = new StringBuilder();
            foreach (var path in Program.PhantomJsXMLManager.CustomJSFilePaths)
            {
                var p = Application.StartupPath + @"\" + path;
                b.Append(FileManager.ReadFromFile(p)).Append("\n");
            }

            return b.ToString();
        }

        private void SetTempFilePaths()
        {
            TempJSFile = Environment.ExpandEnvironmentVariables(Program.MainXMLManager.TempFolderPath
                                                              + "\\" + Guid.NewGuid() + ".js");

            //file to be used for phantomjs execuation
            TempHTMLFile = Environment.ExpandEnvironmentVariables(Program.MainXMLManager.TempFolderPath
                                                              + "\\" + Guid.NewGuid() + ".html");

        }

        #endregion

        PhantomJsXMLRepository PhantomJsXMLRepository { get; set; }

        //todo, not the best implementation, code smell
        public void Init()
        {
            //this.JavascriptError = new StringBuilder();
            this.StandardError = new StringBuilder();
            this.StandardOut = new StringBuilder();

            PhantomJsXMLRepository = Program.PhantomJsXMLManager;

            SetTempFilePaths();
        }

        public PhantomJsHelper()
        {
            Init();
        }

    }

    public class JavaScriptExecuter
    {
        MongoQueryHelper _queryHelper;
        public MongoQueryHelper QueryHelper {
            get
            {
                if (_queryHelper == null) {
                    _queryHelper = new MongoQueryHelper();
                    return _queryHelper;
                }
                return _queryHelper;
            }
            set {
                _queryHelper = value;
            }
        }

        PhantomJsHelper _phantomJsHelper;
        public PhantomJsHelper PhantomJsHelper
        {
            get
            {
                if (_phantomJsHelper == null)
                {
                    _phantomJsHelper = new PhantomJsHelper();
                    return _phantomJsHelper;
                }
                return _phantomJsHelper;
            }
            set
            {
                _phantomJsHelper = value;
            }
        }

        public StringBuilder StandardOut { get; set; }
        public StringBuilder StandardError { get; set; }

        public void ResetOutputs()
        {
            StandardOut = Program.ProgramMode == Program.Mode.Mongo
                ? QueryHelper.StandardOut
                : PhantomJsHelper.StandardOut;

            StandardError = Program.ProgramMode == Program.Mode.Mongo
                ? QueryHelper.StandardError
                : PhantomJsHelper.StandardError;
        }

        public String ExecuteMongo(string query)
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
            //execute file
            String arguments = String.Format(
                "{0} --quiet --host {1} {2} ",
                Program.MongoXMLManager.CurrentServer.CurrentDatabase.Name,
                Program.MongoXMLManager.CurrentServer.Name,
                tempFilePath);

            ExecuteConsoleApp("mongo.exe", arguments);

            //delete file
            FileManager.DeleteFile(tempFilePath);

            //return output
            return QueryHelper.QueryOutputAll;
        }
    
        public String ExecutePhantomJs(string htmlContent)
        {
            //reset query helper
            PhantomJsHelper.Init();
            ResetOutputs();


            //check for query
            if (String.IsNullOrWhiteSpace(htmlContent))
            {
                return String.Empty;
            }

            //prepare html and js file
            //todo a bit of refactoring here
            var tempHtmlFilePath = PhantomJsHelper.PrepareHtmlFile(htmlContent);
            var tempFilePath = PhantomJsHelper.PrepareJsFile();

            //actually executing the query using file
            String arguments = String.Format("{0} > a.txt", tempFilePath.Replace(@"\", @"/"));
            ExecuteConsoleApp(Program.PhantomJsXMLManager.ExeFilePath, arguments);

            //delete file
            FileManager.DeleteFile(tempFilePath);
            FileManager.DeleteFile(tempHtmlFilePath);

            //return output
            return PhantomJsHelper.QueryOutputAll;
        }
 
        private void ExecuteConsoleApp(String exeName, String arguments)
        {
            var process = new Process();
            process.StartInfo.FileName = exeName; //"mongo.exe ";

            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = QueryHelper.NoWindows;
            //Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(5000);
            //    if (!process.HasExited)
            //    {
            //        process.Kill();
            //        var errorManager =  new ErrorManagerThreadSafe();
            //        errorManager.Write("process timed out after 5 seconds");
            //        errorManager.CloseForm(10);
            //    }

            //});

            process.Start();

            //---------------------------------------------------------------
            //from msdn
            //---------------------------------------------------------------
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.

            //asyncrhonous
            process.BeginOutputReadLine();
            process.OutputDataReceived += process_OutputDataReceived;
            //syncrhonous
            if (StandardError != null)
            {
                StandardError.Append(process.StandardError.ReadToEnd());
            }
            
            process.WaitForExit();
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (StandardOut != null)
            {
                StandardOut.Append(e.Data + "\r\n");
            }
        }

        //todo need cleanup refactoring etc
        //display output in notepad, excel etc.
        public void DisplayQueryInExe(String content, String exe)
        {
            var tempPath = Environment.ExpandEnvironmentVariables
                (Program.MainXMLManager.TempFolderPath + "\\"
                 + Guid.NewGuid() + ".json");

            if (!FileManager.SaveToFile(tempPath, content))
            {
                return;
            }

            var process = new Process();
            process.StartInfo.FileName = exe;
            process.StartInfo.Arguments = tempPath;
            process.Start();
        }

    }

}
