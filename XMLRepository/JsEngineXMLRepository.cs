using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using DBUI;
using DBUI.DataModel;

namespace DBUI.Queries {

    public class JsEngineXMLRepository : XMLManager
    {
        
        //private const String _lastFilePath = "Miscellaneous/LastOpenedFilePath";
        private const String _lastFilePaths = "Miscellaneous/LastOpenedFilePaths";
        //private const String _FileHistory = "Miscellaneous/FileHistory";
        //private const String _tempFolderPath = "Options/General/TempFolder";
        private const String _queryFolderPath = "Options/Query/QueryFolder";
        private const String _deleteTempFolderContents = "Options/General/DeleteTempFolderContents";
        //private const String _startupFilePath = "Options/General/StartupFile";
        private const String _customJSFilePaths = "Options/CustomJavascriptFiles/Path";
        private const String _queryOutputTypes = "Options/QueryOutputTypes";
        private const String _codeSnippets = "Options/CodeSnippets";

        private String _xmlPath;

        public bool Init(JsEngineType type) {
            _xmlPath = Application.StartupPath + "/" + type.ToString() + ".xml";
            return base.Init(_xmlPath);
        }

        public List<String> CustomJSFilePaths
        {
            get
            {
                List<String> sl = new List<string>();
                XmlNodeList ns = RootNode.SelectNodes(_customJSFilePaths);
                foreach (XmlNode n in ns)
                {
                    sl.Add(n.InnerText);
                }
                return sl;
            }
        }
        
        public List<String> LastOpenedFilePaths
        {
            get
            {
                List<String> sl = new List<string>();
                var ns = RootNode.SelectNodes(_lastFilePaths + "/*").ToList();

                return ns.Select(n => n.InnerText).ToList();
            }
            set
            {
                XmlNode n = RootNode.SelectSingleNode(_lastFilePaths);
                if (n == null)
                {
                    return;
                }

                //remove all nodes from original
                n.RemoveAll();

                //append new nodes
                value.ForEach(v=>this.AppendNode(n, "f", v));
            }
        }
        
    }
}
