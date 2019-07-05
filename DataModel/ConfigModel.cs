using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DBUI.DataModel
{
    public class CodeSnippet {
        public string GroupName { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }

    public class Miscellaneous {
        public List<string> LastOpenedFilePaths {get; set;}
        public string TempFolder {get; set;}
        public bool DeleteTempFolderContents {get; set;}
        public bool AutoComplete { get; set; }
        public List<CodeSnippet> CodeSnippets { get; set; }
        public string QueryFolder { get; set; }
    }

    public class OutputType {
        public string Name { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class ConfigModel
    {
        public List<string> CustomJavascriptFiles { get; set; }
        public List<Server> Servers { get; set; }
        public Miscellaneous Miscellaneous { get; set; }
        public List<OutputType> QueryOutputTypes { get; set; }
        public string MongoClientExePath { get; set; }   
    }
}
