﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBUI.Queries
{
    public class CollectionAutoCompleter
    {
        public List<String> GetCurrentDBCollectionNames() { 
            
            //var server = Program.MongoXMLManager.CurrentServer.Name;
            //var database = Program.MongoXMLManager.CurrentServer.CurrentDatabase.Name;

            if (Program.MainXMLManager.CurrentEngine != JsEngineType.MongoDB)
            {
                return new List<String>();
            }


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
            
            if (Program.MainXMLManager.CurrentEngine != JsEngineType.MongoDB) {
                return;
            }

            var server = Program.JsEngine.MongoEngine.Repository.CurrentServer.Name;
            var database = Program.JsEngine.MongoEngine.Repository.CurrentServer.CurrentDatabase.Name;
            var collectionNames = GetCurrentDBCollectionNames();
            
            collectionNames = collectionNames
                .Select(n => n.Replace("\n\r", ""))
                .Where(m => !String.IsNullOrEmpty(m)).ToList();

            Program.JsEngine.MongoEngine.Repository.SetCollectionList
                        (collectionNames, server, database);
         
        }
    }
}
