using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBUI.DataModel {
    class Server {
        public String Name { get; set; }
        public String StartupFile { get; set; }
        public List<Database> Databases { get; set;}
        public List<String> CustomFilePaths {get; set;}
        //update this to database object
        public Database CurrentDatabase {get; set;}
        public bool WithWarning { get; set; }
    }

    internal class Database
    {
        public String Name { get; set; }
        public List<String> Collections { get; set; }
    }
}
