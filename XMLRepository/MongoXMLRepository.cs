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
   
    public class MongoXMLRepository : JsEngineXMLRepository {
        
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
            return base.Init(JsEngineType.MongoDB);
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
                    s.Databases = GetDatabase(m);
                    servers.Add(s);
                }
                return servers;
            }
        }

        public void SetCollectionList(List<String> l, String server, String database) {
            var serverNode = RootNode.SelectNodes(_servers + "/*").ToList().FirstOrDefault(
                     x => x.SelectSingleNode("@name").Value == server);
            if (serverNode == null) { 
                return; 
            }

            var databaseNode = serverNode.SelectNodes("Database").ToList().FirstOrDefault(
                     x => x.SelectSingleNode("@name").Value == database);

            if (databaseNode == null)
            {
                return;
            }

            var collectionNodeNew = this.CreateNode("Collections", null);
            foreach (var collection in l) {
                var n = this.AppendNode(collectionNodeNew, "C", "");
                var attr = this.AppendAttribute(n, "name", collection);
            }

            var collectionNodeOld = databaseNode.SelectSingleNode("Collections");

            if (collectionNodeOld == null) {
                databaseNode.AppendChild(collectionNodeNew);
                return;
            }

            databaseNode.ReplaceChild(collectionNodeNew, collectionNodeOld);

        }

        private List<Database> GetDatabase(XmlNode serverNode)
        {
            var l = new List<Database>();
            foreach (XmlNode o in serverNode.SelectNodes("*"))
            {
                l.Add(new Database()
                {
                    Name = o.SelectSingleNode("@name").Value,
                    Collections = GetCollectionList(o)
                });
            }
            return l;
        }

        public void AddDatabase(String server, String database)
        {
            var serverNode = RootNode.SelectNodes(_servers + "/*").ToList().FirstOrDefault(
                    x => x.SelectSingleNode("@name").Value == server);
            if (serverNode == null)
            {
                return;
            }

            var databaseNode = this.CreateNode("Database", null);
            this.AppendAttribute(databaseNode, "name", database);
            serverNode.AppendChild(databaseNode);
        }

        public void DeleteDatabase(String server, String database)
        {
            var serverNode = RootNode.SelectNodes(_servers + "/*").ToList().FirstOrDefault(
                    x => x.SelectSingleNode("@name").Value == server);
            if (serverNode == null)
            {
                return;
            }

            var databaseNode = serverNode.SelectNodes("Database").ToList().FirstOrDefault(
                    d =>
                    {
                        var node = d.SelectSingleNode("@name");
                        if (node != null)
                        {
                            return d.SelectSingleNode("@name").Value == database;
                        }
                        else {
                            return false;
                        }
                        
                    });
                
            if (databaseNode == null)
            {
                return;
            }

            serverNode.RemoveChild(databaseNode);
            
        }

        private List<String> GetCollectionList(XmlNode n)
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
    }
}
