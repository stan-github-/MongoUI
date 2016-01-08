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

    public class MessageManager {

        public MessageManager(string queryFilePath, int errorLineNumOffset) {
            this.QueryFilePath = queryFilePath;
            this.QueryErrorLineNumOffset = errorLineNumOffset;
            StandardOut = new StringBuilder();
            StandardError = new StringBuilder();
        }

        public StringBuilder StandardOut { get; set; }
        public StringBuilder StandardError { get; set; }
        public int QueryErrorLineNumOffset { get; set; }
        public string QueryFilePath { get; set; }

        public String StandardErrorAndOut
        {
            get
            {
                return String.Format("{0}\n\r{1}\n\r",
                    StandardError.ToString(),
                    StandardOut.ToString());
            }
        }

        public String GetJavascriptQueryError()
        {
            /*
             * 
             * Sun Mar 29 21:27:37.660 JavaScript execution failed: ReferenceError: 
            zzz is not defined at 
            E:\Users\ztan\AppData\Local\Temp\\69d3425f-150d-4d1d-abab-1be699fabac0.js:L26
            failed to load: E:\Users\ztan\AppData\Local\Temp\\69d3425f-150d-4d1d-abab-1be699fabac0.js
             */

            string str = string.Empty;

            if (StandardOut.ToString().Contains("couldn't connect to server"))
            {
                return "Please start mongo server";
            }

            str = GetErrorMessageWithLineNumber();

            return str;
            
        }
        
        private string GetErrorMessageWithLineNumber(){
            return StandardOut.ToString();
            
            if (!StandardOut.ToString().Contains("JavaScript execution failed:")) {
                return string.Empty;
            }
            
            var array = this.StandardOut.ToString()
                .Split(new string[] { "failed to load:", this.QueryFilePath },
                    StringSplitOptions.RemoveEmptyEntries);

            if (array.Length < 3 || !array[1].Contains(":L"))
            {
                throw new Exception("javascript error message parse error!!, please update mongo and mongo ui!");
            }

            var lineNumber = array[1].Replace(":L", "").Replace("\n\r", "");

            int lineNumberInt = 0;

            if (!int.TryParse(lineNumber, out lineNumberInt))
            {
                throw new Exception("javascript error message parse error!!, please update mongo and mongo ui!");
            }

            var sb = new StringBuilder();
            sb.Append(array[0]).Append(Environment.NewLine);
            sb.Append("line number: ")
                .Append((lineNumberInt - this.QueryErrorLineNumOffset + 1).ToString());

            return sb.ToString();
        }
    }

   
}
