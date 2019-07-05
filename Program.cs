using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DBUI.Queries;
using System.Linq;

namespace DBUI
{
    static class Program
    {
        //public static JsEngineProxy JsEngine;
        public static ConfigManager Config { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        //todo need save file as.
        static void Main()
        {

            //MainXMLManager = new MainXMLRepository();
            //MainXMLManager.Init("main.xml");
            Config  = new ConfigManager("mongoDB.json");

            //JsEngine = new JsEngineProxy();

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