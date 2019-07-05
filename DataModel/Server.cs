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
                    return new Database() { Name = "admin", IsCurrent = true };
                }
                if (Databases.Count == 1) {
                    return Databases[0];
                }
                var db =  Databases.FirstOrDefault(d => d.IsCurrent);
                if (db == null) {
                    return Databases[0];
                }
                return null;
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
