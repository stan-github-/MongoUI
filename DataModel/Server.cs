using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBUI.DataModel {
    
    public class Server {
        public String Name { get; set; }
        public String Alias { get; set; }
        public String StartupFile { get; set; }
        public List<Database> Databases { get; set;}
        public bool WithWarning { get; set; }
        public String User { get; set; }
        public String Password { get; set; }
        public bool IsCurrent { get; set; }

        public Database CurrentDatabase  {
            get {
                if (Databases == null || Databases.Count == 0) {
                    ErrorManager.Write(string.Format("Please set up databases for server {0}", this.Alias));
                    return null;
                }
                if (Databases.Count == 1) {
                    Databases[0].IsCurrent = true;
                    return Databases[0];
                }
                var db =  Databases.FirstOrDefault(d => d.IsCurrent);
                if (db == null) {
                    Databases[0].IsCurrent = true;
                    return Databases[0];
                }
                return db;
            }                                  
        }

    }

    public class Database
    {
        public String Name { get; set; }
        public List<String> Collections { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class SnippetFile {
        public String GroupName { get; set; }
        public String Name { get; set; }
        public String FilePath { get; set; }
    }

}
