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

namespace DBUI.Mongo {
   /* class MongoXMLManagerV2 : XMLManagerV2
    {

        private const String _lastFilePath = "Miscellaneous/LastOpenedFilePath";
        private const String _FileHistory = "Miscellaneous/FileHistory/FilePath";
        private const String _tempFolderPath = "Options/General/TempFolder";
        private const String _queryFolderPath = "Options/Query/QueryFolder";
        private const String _deleteTempFolderContents = "Options/General/DeleteTempFolderContents";
        //private const String _startupFilePath = "Options/General/StartupFile";
        private const String _servers = "Servers/Server";
        private const String _currentServer = "Miscellaneous/CurrentServer";
        private const String _customJSFilePaths = "Options/CustomJavascriptFiles/Path";
        private const String _queryOutputTypes = "Options/QueryOutputTypes";

        private String _xmlPath;

        public bool Init()
        {
            _xmlPath = Application.StartupPath + "/MongoXML.xml";
            return base.Init(_xmlPath, "DocumentElement");
        }

        public List<Server> Servers
        {
            get
            {
                var n = RootNode.XPathSelectElements(_servers);
                var servers = new List<Server>();
                foreach (var m in n) {
                    var s = new Server
                        {
                            Name = m.GetAttributeValue("name"),
                            WithWarning = bool.Parse(m.GetAttributeValue("withWarning")),
                            Databases = new List<String>()
                        };

                    foreach (var o in m.Descendants("Database"))
                    {
                        s.Databases.Add(o.Value);
                    }
                    servers.Add(s);
                }

                return servers;
            }
        }

        //todo need testing
        public bool DeleteTempFolderContents
        {
            set
            {
                var n = RootNode.XPathSelectElements(_deleteTempFolderContents).FirstOrDefault();
                if (n != null) { n.Value = value == true ? "true" : "false"; }
            }
            get
            {
                var n = RootNode.XPathSelectElements(_deleteTempFolderContents).FirstOrDefault();
                if (n != null)
                {
                    return n.Value == "true" ? true : false;
                }
                return false;
            }

        }

        public List<String> FileHistory
        {
            get
            {
                var sl = new List<string>();
                var ns = RootNode.XPathSelectElements(_FileHistory);
                foreach (var n in ns)
                {
                    sl.Add(n.Value);
                }
                return sl;
            }
            set
            {
                var nl = RootNode.XPathSelectElements(_FileHistory);
                var nr = RootNode.XPathSelectElement(_FileHistory);

                if (nr == null)
                {
                    return;
                }

                value.ForEach(v =>
                {
                    if (!nl.Any(m => m.Value == v))
                    {
                        nr.AddFirst(v);
                    }
                });
            }

        }

        public List<String> CustomJSFilePaths
        {
            get
            {
                var ns = RootNode.XPathSelectElements(_customJSFilePaths);
                return ns.Select(n => n.Value).ToList();
            }
        }

        public String LastFilePath
        {
            set
            {
                var n = RootNode.XPathSelectElement(_lastFilePath);
                if (n != null) { n.Value = value; }
            }
            get
            {
                var n = RootNode.XPathSelectElement(_lastFilePath);
                return n != null ? n.Value : null;
            }

        }

        public String TempFolderPath
        {
            set
            {
                var n = RootNode.XPathSelectElement(_tempFolderPath);
                if (n != null) { n.Value = value; }
            }
            get
            {
                var n = RootNode.XPathSelectElement(_tempFolderPath);
                return n != null ? n.Value : null;
            }

        }

        public String QueryFolderPath
        {
            set
            {
                var n = RootNode.XPathSelectElement(_queryFolderPath);
                if (n != null) { n.Value = value; }
            }
            get
            {
                var n = RootNode.XPathSelectElement(_queryFolderPath);
                return n != null ? n.Value : null;
            }

        }

        public Server CurrentServer
        {
            get
            {
                var n = RootNode.XPathSelectElement(_currentServer);
                var s = new Server()
                {
                    Name = n.GetAttributeValue("name"),
                    CurrentDatabase = n.XPathSelectElement("CurrentDatabase").Value,
                };
                return s;
            }
            set
            {
                RootNode.XPathSelectElement(_currentServer).GetAttribute("name").Value = value.Name;
                RootNode.XPathSelectElement(_currentServer + "/CurrentDatabase").Value = value.CurrentDatabase;
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

        public QueryOutputType QueryOutputTypes
        {
            set
            {
                var n = RootNode.XPathSelectElement(_queryOutputTypes);
                if (n == null)
                {
                    return;
                }
                n.Attribute("current").Value = value.CurrentOutputType;

            }
            get
            {
                var n = RootNode.XPathSelectElement(_queryOutputTypes);
                if (n == null)
                {
                    return null;
                }
                
                var q = new QueryOutputType
                {
                    CurrentOutputType = n.Attribute("current").Value
                };

                foreach (var m in n.Descendants("T"))
                {
                    q.Types.Add(m.Value);
                }

                return q;
            }
        }
    }
    */

    class MongoXMLRepository : XMLManager {
        
        private const String _lastFilePath = "Miscellaneous/LastOpenedFilePath";
        private const String _lastFilePaths = "Miscellaneous/LastOpenedFilePaths";
        private const String _FileHistory = "Miscellaneous/FileHistory";
        private const String _tempFolderPath = "Options/General/TempFolder";
        private const String _queryFolderPath = "Options/Query/QueryFolder";
        private const String _deleteTempFolderContents = "Options/General/DeleteTempFolderContents";
        //private const String _startupFilePath = "Options/General/StartupFile";
        private const String _servers = "Servers";
        private const String _currentServer = "Miscellaneous/CurrentServer";
        private const String _customJSFilePaths = "Options/CustomJavascriptFiles/Path";
        private const String _queryOutputTypes = "Options/QueryOutputTypes";
        private const String _sqlcmd = "Options/SQLCmd";
        private const String _codeSnippets = "Options/CodeSnippets";

        private String _xmlPath;

        public bool Init() {
            _xmlPath = Application.StartupPath + "/MongoXML.xml";
            return base.Init(_xmlPath, "DocumentElement");
        }

        public class SQLCmd
        {
            public String ExePath { get; set; }
            public String Server { get; set; }
            public String Database { get; set; }
            public String Query { get; set; }
        }

        public SQLCmd SQlCmd
        {
            get
            {
                XmlNode n = RootNode.SelectSingleNode(_sqlcmd);
                if (n == null)
                {
                    return new SQLCmd();
                }

                var s = n.SelectNodes("*").ToList().FirstOrDefault
                        (x => x.SelectSingleNode("@name").Value 
                        == n.SelectSingleNode("@current").Value);

                return new SQLCmd()
                    {
                        ExePath = s.SelectSingleNode("ExePath").InnerText,
                        Database = s.SelectSingleNode("Database").InnerText,
                        Server = s.SelectSingleNode("Server").InnerText,
                    };
            }
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
                value.ForEach(x => this.AppendNode(ref m, "f", x));

                //append nodes from past list
                foreach (XmlNode x in n.SelectNodes("*"))
                {
                    if (m.SelectSingleNode(String.Format("*[text()='{0}']", x.InnerText)) 
                        == null)
                    {
                        this.AppendNode(ref m, "f", x.InnerText);
                    }
                }

                //remove all nodes from original
                n.RemoveAll();

                //append new nodes
                m.SelectNodes("*").ToList().Take(9).ToList()
                    .ForEach(x => this.AppendNode(ref n, "f", x.InnerText));
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

        public List<Server> Servers
        {
            get
            {
                var n = RootNode.SelectSingleNode(_servers);
                List<Server> servers = new List<Server>();
                foreach (XmlNode m in n.SelectNodes("Server"))
                {
                    var s = new Server()
                    {
                        Name = m.SelectSingleNode("@name").Value,
                        WithWarning = bool.Parse(m.SelectSingleNode("@withWarning").Value)
                    };
                    s.Databases = BuildDatabase(m);
                    servers.Add(s);
                }
                return servers;
            }
        }

        private List<Database> BuildDatabase(XmlNode serverNode)
        {
            var l = new List<Database>();
            foreach (XmlNode o in serverNode.SelectNodes("*"))
            {
                l.Add(new Database()
                {
                    Name = o.SelectSingleNode("@name").Value,
                    Collections = BuildCollectionList(o)
                });
            }
            return l;
        }

        private List<String> BuildCollectionList(XmlNode n)
        {
            if (n == null)
            {
                return null;
            }

            var l = new List<String>();
            foreach (XmlNode o in n.SelectNodes("Collections/*"))
            {
                l.Add(o.SelectSingleNode("@name").Value);
            }
            return l;
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
                value.ForEach(v=>this.AppendNode(ref n, "f", v));
            }
        }

        public Server CurrentServer {
            get {
                XmlNode n = RootNode.SelectSingleNode(_currentServer);
                String database = n.SelectSingleNode("CurrentDatabase").InnerXml;
                Server s = new Server()
                    {
                        Name = n.SelectSingleNode("@name").Value,
                        CurrentDatabase = new Database { Name = database }
                    };
                return s;
            }
            set {
                RootNode.SelectSingleNode(_currentServer + "/@name").Value = value.Name;
                RootNode.SelectSingleNode(_currentServer + "/CurrentDatabase").InnerXml =
                         value.CurrentDatabase.Name;
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
