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

            var executor = new QueryExecuter() { NoFeedBack = true };
            
            //custom function defined in script file!
            
            var func = "GetCollectionNames();";

            var results = executor.Execute(func);
            
            if (!String.IsNullOrWhiteSpace(executor.QueryError)) {
                ErrorManager.Write(executor.QueryError);
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
            var server = Program.MongoXMLManager.CurrentServer.Name;
            var database = Program.MongoXMLManager.CurrentServer.CurrentDatabase.Name;
            var collectionNames = GetCurrentDBCollectionNames();
            Program.MongoXMLManager.SetCollectionList
                        (collectionNames, server, database);
         
        }
    }
}
