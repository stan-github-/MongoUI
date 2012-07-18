using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBUI.DataModel {
    class Server {
        public String Name { get; set; }
        public String StartupFile { get; set; }
        public List<String> Databases { get; set;}
        public List<String> CustomFilePaths {get; set;}
        public String CurrentDatabase {get; set;}
    }
}
