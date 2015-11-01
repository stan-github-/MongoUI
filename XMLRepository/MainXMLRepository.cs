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
   
    public class MainXMLRepository : XMLManager {
        
        private const String _lastFilePath = "Miscellaneous/LastOpenedFilePath";
        private const String _lastFilePaths = "Miscellaneous/LastOpenedFilePaths";
        private const String _FileHistory = "Miscellaneous/FileHistory";
        private const String _tempFolderPath = "Options/General/TempFolder";
        private const String _queryFolderPath = "Options/Query/QueryFolder";
        private const String _deleteTempFolderContents = "Options/General/DeleteTempFolderContents";
        //private const String _startupFilePath = "Options/General/StartupFile";
        //private const String _customJSFilePaths = "Options/CustomJavascriptFiles/Path";
        private const String _queryOutputTypes = "Options/QueryOutputTypes";
        private const String _codeSnippets = "Options/CodeSnippets";

        private String _xmlPath;

        public bool Init() {
            _xmlPath = Application.StartupPath + "/MongoXML.xml";
            return base.Init(_xmlPath, "DocumentElement");
        }

        public List<String> CodeSnippets
        {
            get
            {
                XmlNode n = RootNode.SelectSingleNode(_codeSnippets);
                if (n == null)
                {
                    return new List<String>();
                }

                var currentGroup = n.SelectNodes("*").ToList().FirstOrDefault
                        (x => x.SelectSingleNode("@name").Value
                        == n.SelectSingleNode("@current").Value);

                if (currentGroup == null)
                {
                    return new List<String>();
                }

                return currentGroup.SelectNodes("*").
                    ToList().Select(x=>x.InnerText).ToList();

            }
        }

        public bool DeleteTempFolderContents {
            set {
                XmlNode n =
                    RootNode.SelectSingleNode(_deleteTempFolderContents);
                if (n != null) { n.InnerText = value == true ? "true": "false"; }
            }
            get {
                XmlNode n =
                    RootNode.SelectSingleNode(_deleteTempFolderContents);
                if (n != null) {
                    return n.InnerText == "true" ? true: false;
                }
                return false;
            }

        }

        public List<String> FileHistory
        {
            get
            {
                List<String> sl = new List<string>();
                var ns = RootNode.SelectNodes(_FileHistory + "/*").ToList();

                return ns.Select(n => n.InnerText).ToList();
            }
            set
            {
                XmlNode n = RootNode.SelectSingleNode(_FileHistory);
                if (n == null)
                {
                    return;
                }

                //append nodes from current list
                var m = this.CreateNode("FileHistory", null);
                value.Reverse();
                value.ForEach(x => this.AppendNode(m, "f", x));

                //append nodes from past list
                foreach (XmlNode x in n.SelectNodes("*"))
                {
                    if (m.SelectSingleNode(String.Format("*[text()='{0}']", x.InnerText)) 
                        == null)
                    {
                        this.AppendNode (m, "f", x.InnerText);
                    }
                }

                //remove all nodes from original
                n.RemoveAll();

                //append new nodes
                m.SelectNodes("*").ToList().Take(9).ToList()
                    .ForEach(x => this.AppendNode(n, "f", x.InnerText));
            }

        }

        public String LastFilePath
        {
            set
            {
                XmlNode n =
                    RootNode.SelectSingleNode(_lastFilePath);
                if (n!=null) {n.InnerText = value;}
            }
            get
            {
                XmlNode n =
                    RootNode.SelectSingleNode(_lastFilePath);
                if (n != null)
                {
                    return n.InnerText;
                }
                return null;
            }

        }

        public String TempFolderPath
        {
            set
            {
                XmlNode n = RootNode.SelectSingleNode(_tempFolderPath);
                if (n != null) { n.InnerText = value; }
            }
            get
            {
                XmlNode n =
                    RootNode.SelectSingleNode
                        (_tempFolderPath);
                if (n != null)
                {
                    return n.InnerText;
                }
                return null;
            }

        }

        public String QueryFolderPath {
            set {
                XmlNode n =
                    RootNode.SelectSingleNode
                        (_queryFolderPath);
                if (n != null) { n.InnerText = value; }
            }
            get {
                XmlNode n =
                    RootNode.SelectSingleNode
                        (_queryFolderPath);
                if (n != null) {
                    return n.InnerText;
                }
                return null;
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

        public class QueryOutputType
        {
            public QueryOutputType()
            {
                Types = new List<string>();
            }

            public List<String> Types { get; set; }
            public String CurrentOutputType { get; set; } 
        }

        public QueryOutputType QueryOutputTypes{
           set
            {
                XmlNode n = RootNode.SelectSingleNode(_queryOutputTypes);
                if (n == null)
                {
                    return; 
                }
                n.SelectSingleNode("@current").InnerText = value.CurrentOutputType;
                
            }
            get
            {
                XmlNode n = RootNode.SelectSingleNode(_queryOutputTypes);
                if (n == null)
                {
                    return null;
                }
                QueryOutputType q = new QueryOutputType();
                q.CurrentOutputType = n.SelectSingleNode("@current").Value;
                foreach (XmlNode m in n.SelectNodes("T"))
                {
                    q.Types.Add(m.InnerText);
                }
                return q;
            }
        }
    }
}
