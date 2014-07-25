using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DBUI.Mongo;

namespace DBUI
{
    static class Program
    {
        public static Mongo.MongoXMLRepository MongoXMLManager;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]

        //todo need save file as.
        static void Main()
        {
            MongoXMLManager = new MongoXMLRepository();
            MongoXMLManager.Init("MongoXML.xml", "DocumentElement");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMainMDI());
        }
    }
}