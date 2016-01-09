using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DBUI.Queries;
using System.Linq;

namespace DBUI
{
    static class Program
    {
        public static Queries.MongoXMLRepository MongoXMLManager;
        //public static Queries.PhantomJsXMLRepository PhantomJsXMLManager;
        public static Queries.MainXMLRepository MainXMLManager;

        //public enum Mode
        //{
        //    Mongo, PhantomJs
        //}

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

            var mainXMLManager = new MainXMLRepository();
            mainXMLManager.Init("main.xml");

            var currentServer = mainXMLManager.Engines
                .First(e=>e.IsCurrent == true);
            

            var xmlFile = "Mongo.xml";
            MongoXMLManager = new MongoXMLRepository();
            MongoXMLManager.Init(xmlFile, "DocumentElement");

            //PhantomJsXMLManager = new PhantomJsXMLRepository();
            //PhantomJsXMLManager.Init(xmlFile, "DocumentElement");

            MainXMLManager = new MainXMLRepository();
            MainXMLManager.Init(xmlFile, "DocumentElement");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMainMDI());
        }
    }
}