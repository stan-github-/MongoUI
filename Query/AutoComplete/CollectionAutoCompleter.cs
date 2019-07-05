using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBUI.Queries
{
    public class CollectionAutoCompleter
    {
        public List<String> GetCurrentDBCollectionNames() { 
            
            var executor = new JavaScriptExecuter() { QueryFileManager = new QueryFileManager() };
            //todo, a bit hacky here
            executor.MessageManager = new MessageManager(executor.QueryFileManager.QueryFilePath);

            executor.QueryExecutionConfiguration.NoFeedBack = true;
                
            //custom function defined in script file!
            
            var func = "GetCollectionNames();";

            var results = executor.ExecuteMongo(func);
            
            if (!String.IsNullOrWhiteSpace(executor.MessageManager.GetJavascriptQueryError())) {
                ErrorManager.Write(executor.MessageManager.GetJavascriptQueryError());
            }

            var r = results.Split(
                new String[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
            return r;
        }

        public void RefreshCurrentDBCollectionNames() {
            
            var server = Program.Config.CurrentServer();
            var database = server.CurrentDatabase;
            var collectionNames = GetCurrentDBCollectionNames();
            
            collectionNames = collectionNames
                .Select(n => n.Replace("\n\r", ""))
                .Where(m => !String.IsNullOrEmpty(m)).ToList();

            Program.Config.CurrentServer().CurrentDatabase.Collections = collectionNames;
        }
    }
}
