using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DBUI.Queries;
using System.Linq;
using DBUI.JsEngine;

namespace DBUI
{
    static class Program
    {
        public static Queries.MainXMLRepository MainXMLManager;
        public static JsEngineProxy JsEngine;
        //todo 
        //make this part of main xml settings
        //public static Program.Mode ProgramMode = Mode.Mongo;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        //todo need save file as.
        static void Main()
        {

            MainXMLManager = new MainXMLRepository();
            MainXMLManager.Init("main.xml");

            JsEngine = new JsEngineProxy();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMainMDI());
        }
    }
}

//todo
/*

 * autocomplete broken
 * context switching, saved queries broken


*/