using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBUI.Mongo
{
    class AutoCompleter
    {
        public List<String> GetCollectionNames(String server, String database) { 
            
            //var server = Program.MongoXMLManager.CurrentServer.Name;
            //var database = Program.MongoXMLManager.CurrentServer.CurrentDatabase.Name;

            var executor = new QueryExecuter();
            var func = "GetCollectionNames();";

            var results = executor.Execute(func);
            return results.Split(
                new String[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries)
                .Skip(3).ToList();
        }

        public void RefreshCollectionNames() {
            var servers = Program.MongoXMLManager.Servers;
            foreach (var s in servers) {
                foreach (var d in s.Databases) {
                    var collectionNames = GetCollectionNames(s.Name, d.Name);
                    d.Collections = collectionNames;
                }
            }
        }
    }
}
