using DBUI.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DBUI.Query.AutoComplete
{
    //functions and classes
    public class QueryExecuter
    {
        //add collection names back to database,
        //object intellisense needs to add back the stuff taken out
        //still somewhat buggy with complex queries
        private static JavaScriptExecuter _queryExecuter;
        public static JavaScriptExecuter Executer {
            get {
                if (_queryExecuter == null)
                {
                    _queryExecuter = new JavaScriptExecuter();
                    _queryExecuter.QueryExecutionConfiguration.NoFeedBack = true;
                    return _queryExecuter;
                }
                else {
                    return _queryExecuter;
                }
            }
        }

        private String ReflectionString
        {
            get
            {
                    return @";
                    (function(){
	                    var _x_y_z_1_2_3 = {zzzzzzzzzzzz};
                        printjson('{uuuuuuuuuuuu}');
	                    for (var p in _x_y_z_1_2_3){
		                   printjson(p);
	                    }
                        printjson('{vvvvvvvvvvvv}');
                    })();
                ";
                
            }
        }
        
        public static List<String> Main(String queryFirstHalf, string querySecondHalf)
        {
            var methodOrObjectName = Query.AutoComplete.ObjectChainParser
                .GetMethodOrObjectChainBlock(queryFirstHalf);

            var queryOut = GetReflectionQuery
                (queryFirstHalf, querySecondHalf, methodOrObjectName);

            var output = Executer.ExecuteMongo(queryOut);

            //display error if any
            var error = Executer.MessageManager.GetJavascriptQueryError();
            if (!String.IsNullOrEmpty(error))
            {
                ErrorManager.Write(Executer.MessageManager.GetJavascriptQueryError());
            }

            var properties = GetMethodProperties(output);
            return properties;
        }

        private static List<String> GetMethodProperties(string input) {

            //var output = new QueryExecuter() { NoFeedBack = true }.Execute(input);

            //get the reflection output out of the query output (could contain other stuff)
            var inputList = input.Split
                (new String[] { "{uuuuuuuuuuuu}", "{vvvvvvvvvvvv}" },
                StringSplitOptions.RemoveEmptyEntries).
                ToList();

            if (inputList == null || inputList.Count < 2)
            {
                return new List<string>();
            }

            //split output into a list
            var array = inputList[1]
                .Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim().Replace("\"", ""))
                .Where(s => s.Trim() != String.Empty)
                .OrderBy(s => s).ToList();

            return array;
        }

        public static String GetReflectionQuery
            (String firstHalf, String secondHalf, String objectName)
        {
            var reflectionString = new QueryExecuter().ReflectionString
                .Replace("{zzzzzzzzzzzz}", objectName);

            return String.Format("{0}{1}{2}", firstHalf, reflectionString, secondHalf);
        }

    }
}
