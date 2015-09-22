using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DBUI.Queries
{
    class Scraps
    {
        //#region additional types of execution dos, sql etc, not used!!!
        //public List<Tuple<QueryType, String>> SplitQueries(String query)
        //{
        //    string regex = @"\#\#DOS\#\#|\#\#MONGO\#\#|\#\#SQL\#\#";
        //    var options = RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Singleline;

        //    var tokens = new List<String> { "##DOS##", "##MONGO##", "##SQL##" };
        //    var dict = new List<Tuple<QueryType, String>>();

        //    var reg = new Regex(regex, options);
        //    var s = query.Trim();

        //    try
        //    {
        //        var queries = reg.Split(s);
        //        if (queries == null || queries.Length == 0)
        //        {
        //            return null;
        //        }

        //        if (queries.Length == 1)
        //        {
        //            return new List<Tuple<QueryType, String>>{ 
        //                  new Tuple<QueryType,String>(QueryType.MONGO, queries[0])};
        //        }

        //        var c = reg.Matches(s);

        //        int i = 0;
        //        foreach (Match q in c)
        //        {
        //            //if the first query has no token assume it's mongo
        //            if (i == 0)
        //            {
        //                if (q.Index != 0)
        //                {
        //                    dict.Add(new Tuple<QueryType, String>
        //                        (QueryType.MONGO, queries[0]));
        //                }
        //            }

        //            QueryType queryType;
        //            var queryType_ = q.Groups[0].ToString().Replace("#", "");
        //            if (!Enum.TryParse<QueryType>(queryType_, out queryType))
        //            {
        //                throw new Exception(queryType_ + " is not valid query type");
        //            }

        //            dict.Add(new Tuple<QueryType, String>(
        //                queryType, queries[i + 1]));
        //            i++;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.Write(ex);
        //    }
        //    return dict;
        //}

        //private String Execute2(string query)
        //{
        //    //reset query helper
        //    nQueryHelper.Init();

        //    if (String.IsNullOrWhiteSpace(query))
        //    {
        //        return String.Empty;
        //    }

        //    if (QueryHelper.ContinueWithExecutionAfterWarning() == false)
        //    {
        //        return String.Empty;
        //    }

        //    var queries = SplitQueries(query);

        //    //foreach (Tuple<QueryType, String> q in queries)
        //    //{
        //    //    QueryHelper.SetOutputPrefix(q.Item1.ToString());

        //    //    if (q.Item1 == QueryType.SQL || q.Item1 == QueryType.DOS)
        //    //    {
        //    //        ExecuteExternalQueries(q.Item1, new List<String> { q.Item2 });
        //    //    }
        //    //    else
        //    //    {
        //    //        ExecuteMongo(q.Item2);
        //    //    }
        //    //}

        //    return QueryHelper.QueryOutputAll.ToString();
        //}

        //private void ExecuteDOSCmdLine(String command)
        //{

        //    if (string.IsNullOrWhiteSpace(command))
        //    {
        //        return;
        //    }

        //    var tempFile = "";// QueryHelper.TempBatFile;
        //    FileManager.SaveToFile(tempFile, "");
        //    FileManager.AppendToFile(tempFile, command);

        //    ExecuteConsoleApp(tempFile, String.Empty);
        //    FileManager.DeleteFile(tempFile);
        //}

        //private void ExecuteExternalQueries(QueryType type, List<String> queries)
        //{
        //    String arguments = string.Empty;
        //    String fileName = string.Empty;

        //    var sqlCmd = MongoXMLManager.SQlCmd;

        //    if (type == QueryType.SQL)
        //    {
        //        foreach (var q in queries)
        //        {
        //            arguments = String.Format("-S {0} -d {1} -q \"{2}\"",
        //                sqlCmd.Server, sqlCmd.Database, q);
        //            ExecuteConsoleApp(sqlCmd.ExePath, arguments);
        //        }
        //    }

        //    if (type == QueryType.DOS)
        //    {
        //        foreach (var q in queries)
        //        {
        //            ExecuteDOSCmdLine(q);
        //        }
        //    }
        //}

        //public enum QueryType
        //{
        //    MONGO,
        //    SQL,
        //    DOS
        //}

        //#endregion
    }
}
