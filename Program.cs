using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DBUI.Queries;
using System.Linq;
using System.Security.Permissions;

namespace DBUI
{
    static class Program
    {
        //todo

        //history of files....
        //code snippets....


        //public static JsEngineProxy JsEngine;
        public static ConfigManager Config { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        //todo need save file as.
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main()
        {

            Config = new ConfigManager("mongoDB.json");

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalErrorHandler);
      
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMainMDI());
        }

        static void GlobalErrorHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            ErrorManager.Write(e);
            ErrorManager.Write(e.StackTrace);
        }
    }
}

//todo
/*

 * autocomplete broken
 * context switching, saved queries broken


*/