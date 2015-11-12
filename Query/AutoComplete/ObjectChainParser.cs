﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUI.Queries;
using System.Text.RegularExpressions;

namespace DBUI.Query.AutoComplete
{
    public class ObjectChainParser
    {
        public static String GetMethodOrObjectChainBlock(String query)
        {
            //todo a bit hack refactor
            var isMethod = (IsQueryEndingInClosingParenthesis(query));
            var input =
                isMethod ?
                query :
                //if not query, then an object, adding () so it looks like a method
                query + "()";

            var index = GetChainBlockRecursive(input);
            return query.Substring(index, query.Count() - index);
        }

        // var x = db.test().find().clone() 
        // will return db.test().find().clone(), the chained blocks
        public static int GetChainBlockRecursive(String query)
        {

            var delimiters = GetQueryDelimiters(query);
            var delimiter = FindLastValidDelimiterFromBackToFront(query, delimiters);

            bool hasParent = false;
            int methodIndex;
            methodIndex = GetMethodIndex(delimiter, query, out hasParent);

            if (hasParent)
            {
                methodIndex = GetChainBlockRecursive(query.Substring(0, methodIndex));
            }

            return methodIndex;
        }

        // var x = db.test().find().clone(...) 
        // will return clone(...), one logical  block
        public static Match FindLastValidDelimiterFromBackToFront
            (String query, List<Match> delimiters)
        {
            var bracketDict = new Dictionary<String, String>()
                {
                    {")", "("},  {"}", "{"}, {"]", "["}};
            var quotes = new List<String> { @"""", "'" };
            var closeBrackets = new List<String> { "}", ")", "]" };
            var openBrackets = new List<String> { "{", "(", "[", };

            int length = delimiters.Count;

            var stack = new Stack<Match>();

            Match lastValidDelimiter = null;
            //loop through the delimiters, back to front
            for (int i = length - 1; i > -1; i--)
            {
                var delimiter = delimiters[i].Value;

                //if initial delimiter
                if (i == length - 1)
                {
                    //if starting with open bracket exit!
                    if (openBrackets.Contains(delimiter))
                    {
                        return null;
                    }

                    //push close  bracket on to stack
                    stack.Push(delimiters[i]);
                    continue;
                }

                //if more close bracket, keep pushing onto stack
                if (closeBrackets.Contains(delimiter))
                {
                    stack.Push(delimiters[i]);
                    continue;
                }

                //if it's open bracket, 
                if (openBrackets.Contains(delimiter))
                {
                    //something is wierd, delimiters dont match
                    // ie {(}) 
                    if (bracketDict[stack.First().Value] != delimiter)
                    {
                        return null;
                    }

                    //delimiters match, pop the top one
                    var popped = stack.Pop();

                    //set last valid delimiter to current delimiter
                    lastValidDelimiter = delimiters[i];

                    //if stack has gone to zero, break;
                    if (stack.Count() == 0)
                    {
                        break;
                    }

                }

            }

            return lastValidDelimiter;
        }

        private static int GetMethodIndex(Match firstBracket, String query, out bool hasParent)
        {
            //var word = new List<char>();
            var chars = query.ToArray<char>();
            hasParent = false;

            int i;

            for (i = firstBracket.Index - 1; i > -1; i--)
            {
                var c = chars[i];

                if (c == ']' || c == '}' || c == ')')
                {
                    hasParent = true;
                    return i + 1;
                }

                if (Char.IsLetter(c) || Char.IsNumber(c)
                    || Char.IsWhiteSpace(c) || c == '.' || c == '_')
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            return i + 1;
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

        public static List<Match> GetQueryDelimiters(String s)
        {
            string regex = @"\{|\}|\(|\)|\'|\""|\[|\]";
            var options = RegexOptions.IgnorePatternWhitespace |
                RegexOptions.IgnoreCase | RegexOptions.Singleline;
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

        //private static List<Match> RemoveMatchingQuotes(
        //    List<Match> delimiters, out int itemsTaken, out bool matchFound)
        //{
        //    var quotes = new List<String> { @"""", "'" };
        //    if (delimiters.Count == 0 ||
        //        !quotes.Contains(delimiters.Last().Groups[0].ToString()))
        //    {
        //        itemsTaken = 0;
        //        matchFound = true;
        //        return delimiters;
        //    }

        //    int length = delimiters.Count;
        //    var toRemove = new List<Match>();

        //    Match closeDelimiter = null;
        //    Match delimiter = null;

        //    String closeDelimiterString = String.Empty;
        //    String delimiterString = String.Empty;

        //    for (int i = length - 1; i > -1; i--)
        //    {
        //        delimiter = delimiters[i];
        //        delimiterString = delimiter.Groups[0].ToString();

        //        first delimiter,
        //        if (i == length - 1)
        //        {
        //            closeDelimiter = delimiter;
        //            closeDelimiterString = closeDelimiter.Groups[0].ToString();
        //            toRemove.Add(closeDelimiter);
        //            continue;
        //        }

        //        if close quote, return items
        //        if (delimiterString == closeDelimiterString)
        //        {
        //            toRemove.Add(delimiter);
        //            itemsTaken = toRemove.Count;
        //            matchFound = true;
        //            return toRemove;
        //        }
        //        else
        //        {
        //            toRemove.Add(delimiter);
        //            continue;
        //        }
        //    }

        //    itemsTaken = length;
        //    matchFound = false;
        //    return toRemove;
        //}

        //recursively remove brackets
        //find(
        //        function()
        //          {{...}, {...}, [...]}
        //     ).

        //private static List<Match> RemoveMatchingDelimitersRecursivelyBackToFront
        //    (List<Match> delimiters, out int itemsTaken, out bool matchFound)
        //{
        //    int length = delimiters.Count;
        //    var bracketDict = new Dictionary<String, String>()
        //        {
        //            {")", "("},  {"}", "{"}, {"]", "["}
        //        };
        //    var quotes = new List<String> { @"""", "'" };

        //    var closeBrackets = new List<String> { "}", ")", "]" };

        //    var delimitersToRemove = new List<Match>();

        //    Match closeDelimiter = null;
        //    Match delimiter = null;

        //    String closeDelimiterString = String.Empty;
        //    String delimiterString = String.Empty;

        //    //loop through the delimiters
        //    for (int i = length - 1; i > -1; i--)
        //    {
        //        delimiter = delimiters[i];
        //        delimiterString = delimiter.Groups[0].ToString();

        //        //first bracket, assume it's a close bracket
        //        if (i == length - 1)
        //        {
        //            closeDelimiter = delimiter;
        //            closeDelimiterString = closeDelimiter.Groups[0].ToString();
        //            delimitersToRemove.Add(delimiter);
        //            continue;
        //        }

        //        //if open bracket, return items
        //        if (bracketDict.Keys.Contains(closeDelimiterString) &&
        //            bracketDict[closeDelimiterString] == delimiterString)
        //        {
        //            delimitersToRemove.Add(delimiter);
        //            itemsTaken = delimitersToRemove.Count;
        //            matchFound = true;
        //            return delimitersToRemove;
        //        }

        //        //if hits quotes, call remvoing matching quotes
        //        if (quotes.Contains(delimiterString))
        //        {
        //            int itemsToSkip;
        //            delimitersToRemove.AddRange(
        //                RemoveMatchingQuotes(
        //                delimiters.Take(i + 1).ToList(),
        //                out itemsToSkip,
        //                out matchFound));
        //            i = i - itemsToSkip + 1;
        //            continue;
        //        }

        //        //if hits close bracket, remove matching bracket
        //        if (closeBrackets.Contains(delimiterString))
        //        {
        //            int itemsToSkip;
        //            delimitersToRemove.AddRange(
        //                RemoveMatchingDelimitersRecursivelyBackToFront
        //                (delimiters.Take(i + 1).ToList(),
        //                out itemsToSkip,
        //                out matchFound));

        //            i = i - itemsToSkip + 1;
        //            continue;
        //        }
        //        else
        //        {
        //            delimitersToRemove.Add(delimiter);
        //            continue;
        //        }
        //    }

        //    matchFound = false;
        //    itemsTaken = length;
        //    return delimitersToRemove;
        //}
    }
}