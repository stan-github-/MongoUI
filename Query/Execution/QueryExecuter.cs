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
        public QueryFileManager QueryFileManager { get; set; }
        public MessageManager MessageManager {get; set;}
        public QueryExecutionConfiguration QueryExecutionConfiguration { get; set; }

        public JavaScriptExecuter()
        {
            QueryFileManager = new QueryFileManager();
            QueryExecutionConfiguration = new QueryExecutionConfiguration();
        }

        //todo should be in jsengine or jsengine proxy
        public String Execute(string query){
            return ExecuteMongo(query);
        }

        public String ExecuteMongo(string query)
        {
            //check for query
            if (String.IsNullOrWhiteSpace(query))
            {
                return String.Empty;
            }

            //warning check (running against production for example)
            if (this.QueryExecutionConfiguration.ContinueWithExecutionAfterWarning() == false)
            {
                return String.Empty;
            }

            //reset query helper
            this.QueryFileManager.Init();

            //prepend custome js files etc and return file path
            var queryFilePath = QueryFileManager.PrepareJsFile(query);

            //message manager;
            MessageManager.Reset();
            MessageManager.QueryErrorLineNumOffset = QueryFileManager.GetQueryErrorLineNumOffset();
            
            //actually executing the query using file
            //execute file
            var currentServer = Program.Config.CurrentServer();
            String arguments = String.Format(
                "{0} --quiet --host {1} --username {2} --password {3} {4}",
                currentServer.CurrentDatabase.Name,
                currentServer.Name,
                currentServer.User,
                currentServer.Password,
                queryFilePath);

            if (string.IsNullOrWhiteSpace(currentServer.User) || string.IsNullOrWhiteSpace(currentServer.Name)){
                arguments = String.Format(
                "{0} --quiet --host {1} {2}",
                currentServer.CurrentDatabase.Name,
                currentServer.Name,
                queryFilePath);
            } 

            ExecuteConsoleApp(Program.Config.Data.MongoClientExePath, arguments);

            //delete file
            FileManager.DeleteFile(queryFilePath);

            //return output
            return MessageManager.StandardErrorAndOut;
        }


        public String ExecuteNode(string query)
        {
            //check for query
            if (String.IsNullOrWhiteSpace(query))
            {
                return String.Empty;
            }

            //reset query helper
            this.QueryFileManager.Init();

            //prepend custome js files etc and return file path
            var queryFilePath = QueryFileManager.PrepareJsFile(query);

            //message manager;
            MessageManager.Reset();
            MessageManager.QueryErrorLineNumOffset = QueryFileManager.GetQueryErrorLineNumOffset();

            //actually executing the query using file
            //execute file
            String arguments = String.Format(
                "{0}", queryFilePath);

            ExecuteConsoleApp("node.exe", arguments);

            //delete file
            FileManager.DeleteFile(queryFilePath);

            //return output
            return MessageManager.StandardErrorAndOut;
        }
    

        private void ExecuteConsoleApp(String exeName, String arguments)
        {
            var process = new Process();

            try
            {
                process.StartInfo.FileName = exeName; //"mongo.exe ";

                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = this.QueryExecutionConfiguration.NoWindows;

                process.Start();
            }
            catch (Exception e) {
                ErrorManager.Write(e);
                return;
            }
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

        //----------------------------------------------------------------------
        //NOT complete does not support redirection of errors etc..
        //----------------------------------------------------------------------
        //todo need cleanup refactoring etc
        //display output in notepad, excel etc.
        //----------------------------------------------------------------------
        public void DisplayQueryInExe(String content, String exe)
        {
            var tempPath = Environment.ExpandEnvironmentVariables
                (Program.Config.Data.Miscellaneous.QueryFolder + "\\"
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
