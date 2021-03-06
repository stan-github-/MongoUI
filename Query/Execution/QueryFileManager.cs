﻿using System;
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
    public class QueryFileManager {

        public String QueryFilePath { get; set; }
        private String CustomeJsCode { get; set; }

        public void Init()
        {
            SetTempFilePaths();
        }

        public QueryFileManager()
        {
            Init();
        }
        
        public String PrepareJsFile(String query)
        {
            //apppend custom code to file
            this.CustomeJsCode = PrependCustomJSCode();
            FileManager.SaveToFile(this.QueryFilePath, this.CustomeJsCode);
            FileManager.AppendToFile(this.QueryFilePath, query);

            return this.QueryFilePath;
        }

        public int GetQueryErrorLineNumOffset() {
            return this.CustomeJsCode.Split('\n').Length;
        }

        private String PrependCustomJSCode()
        {
            var b = new StringBuilder();
            foreach (var path in Program.Config.Data.CustomJavascriptFiles)
            {
                b.Append(FileManager.ReadFromFile(path)).Append("\n");
            }

            return b.ToString();
        }

        private void SetTempFilePaths()
        {
            if (!String.IsNullOrEmpty(QueryFilePath)) {
                return;
            }

            QueryFilePath = Environment.ExpandEnvironmentVariables
                (Program.Config.Data.Miscellaneous.QueryFolder
                + "\\" + Guid.NewGuid() + ".js");    
        }
    }
}
