using DBUI.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Test.TestMethodFinder();
        }
    }

    public static class Test
    {
        public static void TestMethodFinder() {
            var query = "var x = db.test(function(){var x = function(){return ['a','cb','c'];}}).find(function(){})";
            var method = ObjectAutoCompleter.GetMethodChain(query);
            Console.WriteLine(method);
            Console.Read();
        }

        public static QueryExecuter QueryExecuter {
            get {
                var mongoXMLManager = new MongoXMLRepository();
                mongoXMLManager.Init("MongoXML.xml", "DocumentElement");

                return new QueryExecuter(mongoXMLManager) { NoOutputPrefix = true };
            }
        }
     
        public static void CaseOne(){
            var s = @"var x = db.test.find()";

            String query = String.Empty;
            AutoCompleter.GetReflectionQuery(s, "", out query);
            Console.Write(query);
            
            var output = QueryExecuter.Execute(query);

            var array = output.Split('\r').ToArray()[9].Trim().Replace("\"", "");
            Console.Write(output);
            Console.ReadKey();
        }

        public static void CaseTwo()
        {
            var s = @"function xxx() { var x = db.test.find()";

            String query = String.Empty;
            AutoCompleter.GetReflectionQuery(s, "", out query);
            Console.Write(query);

            var output = QueryExecuter.Execute(query + "}; zzz();");

            Console.Write(output);
            Console.ReadKey();
        }
    }

    public class ErrorManager {
        public static void Write(Exception ex){
            Console.Write(ex.InnerException);
            Console.Write(ex.StackTrace);
        }

        public static void Write(String s) {
            Console.Write(s + Environment.NewLine);
        }
    }

    //todo case one
    //  find method
    //  insert reflection function
    //hum what to do with this???
    /*
     function test2(){
	    function test() {
		    var z = db.test.find();

		    (function(){
			    var x = db.test.find();
			    for (p in x){
				    printz(p);
			    }
		    })();
	    };
	    test();
    };

    test2(); 
     */

    public class AutoCompleter {
        static String ReflectionString =
                @";
                    (function(){
	                var x = {zzzz};
	                    for (p in x){
		                    printz(p);
	                    }
                    })();
                ";
        static bool Debug = true;

        public static List<String> GetMethodArray(String query) {
            String queryOut;
            GetReflectionQuery(query, "", out queryOut);
            
            var output = new QueryExecuter().Execute(queryOut);

            var array = output.Split('\r').ToList();
            array.ForEach(s => s.Trim().Replace("\"", ""));
            return array;
        }

        public static bool GetReflectionQuery
            (String firstHalf, String secondHalf, out String query) {
            
            query = String.Empty;
            if (!IsQueryEndingInClosingParenthesis(firstHalf)) {
                return false;
            }

            var methodName = GetMethodName(firstHalf);

            var reflectionString = AutoCompleter.ReflectionString
                .Replace("{zzzz}", methodName + "()");

            query = String.Format("{0}{1}{2}", firstHalf, reflectionString, secondHalf);
            return true;
        }

        /// <summary>
        /// main get method name function
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static String GetMethodName(String query) {
            var brackets = GetQueryDelimiters(query);
            int itemsTaken;
            bool matchFound = false;

            var bracketsToRemove = RemoveMatchingBrackets
                (brackets, out itemsTaken, out matchFound).OrderBy(x => x.Index);

            String methodName = String.Empty;
            if (matchFound == true)
            {
                methodName = GetMethodName(bracketsToRemove.First(), query);
            }

            if (Debug)
            {
                foreach (var b in bracketsToRemove.OrderBy(x => x.Index))
                {
                    ErrorManager.Write(b.Index.ToString());
                    ErrorManager.Write(b.Groups[0].ToString());
                }

                ErrorManager.Write(matchFound ? "match found" : "not matching delimiters");
                ErrorManager.Write(methodName);
            }
            return methodName;
        }

        private static String GetMethodName(Match firstBracket, String s)
        {
            var word = new List<char>();
            var chars = s.ToArray<char>();

            for (int i = firstBracket.Groups[0].Index - 1; i > -1; i--)
            {
                var c = chars[i];
                if (Char.IsLetter(c) || Char.IsNumber(c) 
                    || Char.IsWhiteSpace(c) || c == '.')
                {
                    word.Add(c);
                }
                else
                {
                    break;
                }
            }

            var Word = new StringBuilder();
            word.Reverse();
            word.ForEach(x => Word.Append(x.ToString()));

            return Word.ToString().Trim();
        }

        public static bool IsQueryEndingInClosingParenthesis(String s)
        {
            //var s = text_box.Text.Substring(0, text_box.CurrentPos - 1);
            //catch "db .   temp", "db.temp", "db. temp", "db .temp"
            string regex = @"\)([\s, \t, \r, \n]*)$";
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


        //delimiters = quotes + brackets
        private static List<Match> GetQueryDelimiters(String s)
        {
            string regex = @"\{|\}|\(|\)|\'|\""|\[|\]";
            var options = RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var reg = new Regex(regex, options);
            try
            {
                var delimiters = reg.Split(s);

                var c = reg.Matches(s);
                var list = new List<Match>();

                foreach (Match q in c)
                {
                    list.Add(q);
                }
                return list;
            }
            catch (Exception ex)
            {
                ErrorManager.Write(ex);
            }

            return new List<Match>();
        }

        private static List<Match> RemoveMatchingQuotes(List<Match> delimiters, out int itemsTaken, out bool matchFound)
        {
            var quotes = new List<String> { @"""", "'" };
            if (delimiters.Count == 0 ||
                !quotes.Contains(delimiters.Last().Groups[0].ToString()))
            {
                itemsTaken = 0;
                matchFound = true;
                return delimiters;
            }

            int length = delimiters.Count;
            var toRemove = new List<Match>();

            Match closeDelimiter = null;
            Match delimiter = null;

            String closeDelimiterString = String.Empty;
            String delimiterString = String.Empty;

            for (int i = length - 1; i > -1; i--)
            {
                delimiter = delimiters[i];
                delimiterString = delimiter.Groups[0].ToString();

                //first delimiter,
                if (i == length - 1)
                {
                    closeDelimiter = delimiter;
                    closeDelimiterString = closeDelimiter.Groups[0].ToString();
                    toRemove.Add(closeDelimiter);
                    continue;
                }

                //if close quote, return items
                if (delimiterString == closeDelimiterString)
                {
                    toRemove.Add(delimiter);
                    itemsTaken = toRemove.Count;
                    matchFound = true;
                    return toRemove;
                }
                else
                {
                    toRemove.Add(delimiter);
                    continue;
                }
            }

            itemsTaken = length;
            matchFound = false;
            return toRemove;
        }

        //recursively remove brackets
        //find(
        //        function()
        //          {{...}, {...}, [...]}
        //     ).
        private static List<Match> RemoveMatchingBrackets(List<Match> delimiters, out int itemsTaken, out bool matchFound)
        {
            int length = delimiters.Count;
            var bracketDict = new Dictionary<String, String>()
                {
                    {")", "("},  {@"}", @"{"}, {@"]", @"["}
                };
            var quotes = new List<String> { @"""", "'" };

            var closeBrackets = new List<String> { @"}", @")", @"]" };
            var toRemove = new List<Match>();

            Match closeDelimiter = null;
            Match delimiter = null;

            String closeDelimiterString = String.Empty;
            String delimiterString = String.Empty;

            for (int i = length - 1; i > -1; i--)
            {
                delimiter = delimiters[i];
                delimiterString = delimiter.Groups[0].ToString();
                //first bracket, assume it's a close bracket
                if (i == length - 1)
                {
                    closeDelimiter = delimiter;
                    closeDelimiterString = closeDelimiter.Groups[0].ToString();
                    toRemove.Add(delimiter);
                    continue;
                }

                //if open bracket, return items
                if (bracketDict.Keys.Contains(closeDelimiterString) &&
                    bracketDict[closeDelimiterString] == delimiterString)
                {
                    toRemove.Add(delimiter);
                    itemsTaken = toRemove.Count;
                    matchFound = true;
                    return toRemove;
                }

                //if hits quotes, call remvoing matching quotes
                if (quotes.Contains(delimiterString))
                {
                    int itemsToSkip;
                    toRemove.AddRange(
                        RemoveMatchingQuotes(delimiters.Take(i + 1).ToList(), out itemsToSkip,
                        out matchFound));
                    i = i - itemsToSkip + 1;
                    continue;
                }

                //if hits close bracket, remove matching bracket
                if (closeBrackets.Contains(delimiterString))
                {
                    int itemsToSkip;
                    toRemove.AddRange(
                        RemoveMatchingBrackets(delimiters.Take(i + 1).ToList(), out itemsToSkip, out matchFound));
                    i = i - itemsToSkip + 1;
                    continue;
                }
                else
                {
                    toRemove.Add(delimiter);
                    continue;
                }
            }

            matchFound = false;
            itemsTaken = length;
            return toRemove;
        }
    }
}
