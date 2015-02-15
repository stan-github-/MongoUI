using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DBUI.Queries.AutoComplete
{
    public class MongoMethods
    {
        //            function test(){
        //    var x = db.c1;
        //    var a = [];
        //    for ( p in db.c1){
        //        a.push(p);
        //    }
        //    var z = _.sortBy(a, function(n){return n;});

        //    printz(z);
        //}
        public readonly static List<String> CollectionObjectMethods =
            new List<String> { 
                    //"_db",
                    //"_dbCommand",
                    //"_distinct",
                    //"_fullName",
                    //"_genIndexName",
                    //"_indexSpec",
                    //"_massageObject",
                    //"_mongo",
                    //"_printExtraInfo",
                    //"_shortName",
                    //"_validateForStorage",
                    //"_validateObject",
                    "aggregate",
                    "clean",
                    "convertToCapped",
                    "convertToSingleObject",
                    "copyTo",
                    "count",
                    "createIndex",
                    "dataSize",
                    "diskStorageStats",
                    "distinct",
                    "drop",
                    "dropIndex",
                    "dropIndexes",
                    "ensureIndex",
                    "exists",
                    "find",
                    "findAndModify",
                    "findOne",
                    "getCollection",
                    "getDB",
                    "getDiskStorageStats",
                    "getFullName",
                    "getIndexKeys",
                    "getIndexSpecs",
                    "getIndexStats",
                    "getIndexes",
                    "getIndices",
                    "getMongo",
                    "getName",
                    "getPagesInRAM",
                    "getQueryOptions",
                    "getShardDistribution",
                    "getShardVersion",
                    "getSlaveOk",
                    "getSplitKeysForChunks",
                    "group",
                    "groupcmd",
                    "help",
                    "indexStats",
                    "insert",
                    "isCapped",
                    "mapReduce",
                    "pagesInRAM",
                    "reIndex",
                    "remove",
                    "renameCollection",
                    "runCommand",
                    "save",
                    "setSlaveOk",
                    "shellPrint",
                    "stats",
                    "storageSize",
                    "toString",
                    "tojson",
                    "totalIndexSize",
                    "totalSize",
                    "update",
                    "validate",
                    "verify"
                };

        public static bool IsQueryEndingInCollectionName(String s)
        {
            //catched "db .   temp", "db.temp", "db. temp", "db .temp"
            //preceded by " ", "=", "(", or "{" or ";"
            //or at the begining of line

            //use [] instead of | to group characters.
            string regex = @"(^|(\s)+|\=|\(|\{|\;)(db)(\s)*\.(\s)*([a-zA-Z0-9]*)$";
            var options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var reg = new Regex(regex, options);
            try
            {
                var c = reg.Matches(s);
                return c.Count > 0;
            }
            catch (Exception ex)
            {
                ErrorManager.Write(ex);
            }
            return false;
        }

        public static bool IsQueryEndingInDB(String s)
        {
            //catched "db .   temp", "db.temp", "db. temp", "db .temp"
            //preceded by " ", "=", "(", or "{" or ";"
            //or at the begining of line

            string regex = @"(^|(\s)+|\=|\(|\{|\;)(db)(\s)*$";
            var options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var reg = new Regex(regex, options);
            try
            {
                var c = reg.Matches(s);
                return c.Count > 0;
            }
            catch (Exception ex)
            {
                ErrorManager.Write(ex);
            }
            return false;
        }

        public static List<String> GetCollectionNames()
        {
            var currentServer = Program.MongoXMLManager.CurrentServer;
            List<String> collections = null;
            try
            {
                collections = Program.MongoXMLManager.Servers
                    .First(z => z.Name == currentServer.Name)
                    .Databases.First(z => z.Name == currentServer.CurrentDatabase.Name)
                    .Collections;
            }
            catch (Exception ex)
            {
                ErrorManager.Write(ex);
            }
            return collections;
        }

    }
}
