using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DBUI;
using DBUI.DataModel;

namespace DBUI.Mongo {
    class MongoXMLManager : XMLManager {
        
        private const String _lastFilePath = "Miscellaneous/LastOpenedFilePath";
        private const String _tempFolderPath = "Options/General/TempFolder";
        private const String _queryFolderPath = "Options/Query/QueryFolder";
        private const String _deleteTempFolderContents = "Options/General/DeleteTempFolderContents";
        private const String _startupFilePath = "Options/General/StartupFile";
        private const String _servers = "Servers";
        private const String _currentServer = "Miscellaneous/CurrentServer";
        private const String _customJSFilePaths = "Options/CustomJavascriptFiles";

        private String _xmlPath;

        public bool Init() {
            _xmlPath = Application.StartupPath + "/MongoXML.xml";
            return base.Init(_xmlPath, "DocumentElement");
        }

        public List<Server> Servers {
            get {
                XmlNode n = RootNode.SelectSingleNode(_servers);
                List<Server> servers = new List<Server>();
                foreach(XmlNode m in n.SelectNodes("Server")){
                    var s = new Server() { Name = m.SelectSingleNode("@name").Value };
                    s.Databases = new List<String>();
                    foreach (XmlNode o in m.SelectNodes("Database")){
                        s.Databases.Add(o.InnerXml);
                    }
                    servers.Add(s);
                }
               return servers;
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

        public List<String> CustomJSFilePaths
        {
            get
            {
                List<String> sl = new List<string>();
                XmlNodeList ns = RootNode.SelectNodes(_customJSFilePaths);
                foreach(XmlNode n in ns)
                {
                    sl.Add(n.InnerText);
                }
                return sl;
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
                XmlNode n =
                    RootNode.SelectSingleNode
                        (_tempFolderPath);
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

        public Server CurrentServer {
            get {
                XmlNode n = RootNode.SelectSingleNode(_currentServer);
                String database = n.SelectSingleNode("CurrentDatabase").InnerXml;
                Server s = new Server()
                    {Name = n.SelectSingleNode("@name").Value, 
                     CurrentDatabase = database};
                return s;
            }
            set {
                RootNode.SelectSingleNode(_currentServer + "/@name").Value = value.Name;
                RootNode.SelectSingleNode(_currentServer + "/CurrentDatabase").InnerXml =
                         value.CurrentDatabase;
            }
        }
   
    }
}
