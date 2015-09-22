using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBUI.Queries
{
    public class JavaScriptExecuter
    {
        MongoQueryHelper _queryHelper;
        public MongoQueryHelper QueryHelper {
            get
            {
                if (_queryHelper == null) {
                    _queryHelper = new MongoQueryHelper();
                    return _queryHelper;
                }
                return _queryHelper;
            }
            set {
                _queryHelper = value;
            }
        }
        public MessageManager MessageManager {get; set;}

        public String ExecuteMongo(string query)
        {
            //reset query helper
            QueryHelper.Init();

            //check for query
            if (String.IsNullOrWhiteSpace(query))
            {
                return String.Empty;
            }

            //warning check (running again production for example)
            if (QueryHelper.ContinueWithExecutionAfterWarning() == false)
            {
                return String.Empty;
            }

            //prepend custome js files etc and return file path
            var queryFilePath = QueryHelper.PrepareJsFile(query);

            //message manager;
            MessageManager = new MessageManager
                (queryFilePath, QueryHelper.GetQueryErrorLineNumOffset());
            
            //actually executing the query using file
            //execute file
            String arguments = String.Format(
                "{0} --quiet --host {1} {2} ",
                Program.MongoXMLManager.CurrentServer.CurrentDatabase.Name,
                Program.MongoXMLManager.CurrentServer.Name,
                queryFilePath);

            ExecuteConsoleApp("mongo.exe", arguments);

            //delete file
            FileManager.DeleteFile(queryFilePath);

            //return output
            return MessageManager.StandardErrorAndOut;
        }
    

        private void ExecuteConsoleApp(String exeName, String arguments)
        {
            var process = new Process();
            process.StartInfo.FileName = exeName; //"mongo.exe ";

            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = QueryHelper.NoWindows;
            
            process.Start();

            //---------------------------------------------------------------
            //from msdn
            //---------------------------------------------------------------
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.

            //asyncrhonous
            process.BeginOutputReadLine();
            process.OutputDataReceived += process_OutputDataReceived;
            //syncrhonous
            if (MessageManager.StandardError != null)
            {
                MessageManager.StandardError
                    .Append(process.StandardError.ReadToEnd());
            }
            
            process.WaitForExit();
        }

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (MessageManager.StandardOut != null)
            {
                MessageManager.StandardOut.Append(e.Data + "\r\n");
            }
        }

        //todo need cleanup refactoring etc
        //display output in notepad, excel etc.
        public void DisplayQueryInExe(String content, String exe)
        {
            var tempPath = Environment.ExpandEnvironmentVariables
                (Program.MainXMLManager.TempFolderPath + "\\"
                 + Guid.NewGuid() + ".json");

            if (!FileManager.SaveToFile(tempPath, content))
            {
                return;
            }

            var process = new Process();
            process.StartInfo.FileName = exe;
            process.StartInfo.Arguments = tempPath;
            process.Start();
        }

    }

}
