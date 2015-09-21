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

        //public String PrepareHtmlFile(String text)
        //{
        //    FileManager.SaveToFile(this.TempJSFile, text);
        //    return this.TempJSFile;
        //}

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

}
