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

            if (Program.JsEngine.CurrentType != JsEngineType.MongoDB)
            {
                return new List<String>();
            }
            

            var executor = new JavaScriptExecuter() ;
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
            //var servers = Program.MongoXMLManager.Servers;
            //foreach (var s in servers) {
            //    foreach (var d in s.Databases) {

            if (Program.JsEngine.CurrentType != JsEngineType.MongoDB) {
                return;
            }

            var server = Program.JsEngine.MongoXMLRepository.CurrentServer.Name;
            var database = Program.JsEngine.MongoXMLRepository.CurrentServer.CurrentDatabase.Name;
            var collectionNames = GetCurrentDBCollectionNames();
            Program.JsEngine.MongoXMLRepository.SetCollectionList
                        (collectionNames, server, database);
         
        }
    }
}
