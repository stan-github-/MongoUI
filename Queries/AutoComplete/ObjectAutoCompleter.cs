using DBUI.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DBUI.Queries
{
    //functions and classes
    public class ObjectAutoCompleter
    {
        //todo
        //override internal print, get rid of printz!!!
        
        //make underscore intellisence, 
        //modify findmethod class to handle string without brackets

        private readonly static String ReflectionString =
                @";
                    (function(){
	                var x = {zzzz};
                        printz('{uuuuuuuuuuuu}');
	                    for (p in x){
		                    printz(p);
	                    }
                        printz('{vvvvvvvvvvvv}');
                    })();
                ";
        private static bool Debug = false;

        public static List<String> Main(String queryFirstHalf, string querySecondHalf)
        {
            var isMethod =  (IsQueryEndingInClosingParenthesis(queryFirstHalf));
            String methodOrObjectName = 
                isMethod?
                GetMethodChain(queryFirstHalf): 
                GetMethodChain(queryFirstHalf + "()");
            
            var queryOut = GetReflectionQuery
                (queryFirstHalf, querySecondHalf, methodOrObjectName, isMethod);

            var output = new QueryExecuter() { NoFeedBack = true}.Execute(queryOut);

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
                .ToList();

            //replace quotes with blank
            var o = new List<String>();
            array.ForEach(s => o.Add(s.Trim().Replace("\"", "")));

            return o.OrderBy(s => s).ToList();
        }

        public static String GetReflectionQuery
            (String firstHalf, String secondHalf, String objectName, bool isMethod)
        {

            var reflectionString = ObjectAutoCompleter.ReflectionString
                .Replace("{zzzz}", objectName + (isMethod?"()":""));

            return String.Format("{0}{1}{2}", firstHalf, reflectionString, secondHalf);
        }

        /// <summary>
        /// var x = a.find({***}).
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static String GetMethodChain(String query)
        {
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
                    || Char.IsWhiteSpace(c) || c == '.' || c == '_')
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

        //public static bool IsQueryEndingInUnderScore(String s)
        //{
        //    //catched " _ . "
        //    //preceded by " ", "=", "(", or "{" or ";"
        //    //or at the begining of line

        //    //            string regex = @"(^|(\s)+|\=|\(|\{|\;)
        //    //                             \_
        //    //                             ([\s, \t, \r, \n]*)
        //    //                             $";
            
        //    var regex = @"(^|(\s)+|\=|\(|\{|\;)_([\s, \t, \r, \n]*)$";
        //    var options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
        //    var reg = new Regex(regex, options);
        //    try
        //    {
        //        var c = reg.Matches(s);
        //        return c.Count > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.Write(ex);
        //    }
        //    return false;
        //}
        ////delimiters = quotes + brackets
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
