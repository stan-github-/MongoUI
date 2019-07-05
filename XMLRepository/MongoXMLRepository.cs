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

namespace DBUI.Queries
{

    /*public class MongoXMLRepository : JsEngineXMLRepository
    {

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
        private const String _queryFolder = "Options/Query/QueryFolder";
        private const String _autoComplete = "Options/General/AutoComplete";

        private String _xmlPath;

        public bool Init()
        {
            return base.Init(JsEngineType.MongoDB);
        }

        public bool AutoComplete {
            get {
                XmlNode n = RootNode.SelectSingleNode(_autoComplete);
                if (n == null) {
                    return false;
                }
                return n.InnerXml == "true";
            }
            set {
                XmlNode n = RootNode.SelectSingleNode(_autoComplete);
                if (n == null)
                {
                    return;
                }
                n.InnerText = value.ToString();
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
                    var userName = m.SelectSingleNode("@user");
                    var password = m.SelectSingleNode("@password");

                    var s = new Server()
                    {
                        Name = m.SelectSingleNode("@name").Value,
                        WithWarning = bool.Parse(m.SelectSingleNode("@withWarning").Value),
                        User = userName != null ? userName.Value : null,
                        Password = password != null ? password.Value : null,
                        Alias = m.SelectSingleNode("@alias").Value

                    };
                    s.Databases = GetDatabase(m);
                    servers.Add(s);
                }
                return servers;
            }
        }

        public Server CurrentServer
        {
            get
            {
                XmlNode n = RootNode.SelectSingleNode(_currentServer);
                Server serverNode = Servers.Where
                    (s => s.Alias == n.SelectSingleNode("@alias").Value).First();
                String currentDatabase = n.SelectSingleNode("CurrentDatabase").InnerXml;

                serverNode.CurrentDatabase = new Database()
                {
                    Name = currentDatabase
                };
                return serverNode;
            }
            set
            {
                RootNode.SelectSingleNode(_currentServer + "/@alias").Value = value.Alias;
                RootNode.SelectSingleNode(_currentServer + "/CurrentDatabase").InnerXml =
                         value.CurrentDatabase.Name;
            }
        }

        public void SetCollectionList(List<String> l, String server, String database)
        {
            var serverNode = RootNode.SelectNodes(_servers + "/*").ToList().FirstOrDefault(
                     x => x.SelectSingleNode("@name").Value == server);

            if (serverNode == null)
            {
                return;
            }

            var databaseNode = serverNode.SelectNodes("Database").ToList().FirstOrDefault(
                     x => x.SelectSingleNode("@name").Value == database);

            if (databaseNode == null)
            {
                return;
            }

            var collectionNodeNew = this.CreateNode("Collections", null);
            foreach (var collection in l)
            {
                var n = this.AppendNode(collectionNodeNew, "C", "");
                var attr = this.AppendAttribute(n, "name", collection);
            }

            var collectionNodeOld = databaseNode.SelectSingleNode("Collections");

            if (collectionNodeOld == null)
            {
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
                        else
                        {
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

        public enum ServerAttribute
        {
            name,
            alias,
            withWarning,
            isCurrent,
            user,
            password,
        }

        public string GetServerAttribute(String server, ServerAttribute attribute)
        {
            var serverNode = RootNode.SelectNodes(_servers + "/*").ToList().FirstOrDefault(
                    x => x.SelectSingleNode("@name").Value == server);
            if (serverNode == null)
            {
                return null;
            }

            var node = serverNode.SelectSingleNode(String.Format("@{0}", attribute.ToString()));
            if (node == null)
            {
                return null;
            }

            return node.Value;
        }

        public string SetServerAttribute(String server, ServerAttribute attribute, String value)
        {
            var serverNode = RootNode.SelectNodes(_servers + "/*").ToList().FirstOrDefault(
                    x => x.SelectSingleNode("@name").Value == server);
            if (serverNode == null)
            {
                return null;
            }

            var node = serverNode.SelectSingleNode(String.Format("@{0}", attribute.ToString()));
            if (node == null)
            {
                node = this.AppendAttribute(serverNode, attribute.ToString(), value);
            }

            return node.Value = value;
        }

        public string GetQueryFolder()
        {

            var queryFolderPath = RootNode.SelectNodes(_queryFolder).ToList().First();

            if (queryFolderPath == null)
            {
                return null;
            }

            return queryFolderPath.Value;
        }

       
        public void AddSnippetFile(string groupName, string name, string filePath)
        {
            //option, codesnippets
            var group = RootNode.SelectNodes(_codeSnippets + "/*").ToList()
                .FirstOrDefault(g => g.SelectSingleNode("@name").Value == groupName);

            if (group == null)
            {
                group = this.CreateNode("Group", null);
                this.AppendAttribute(group, "name", groupName);
                RootNode.SelectSingleNode(_codeSnippets).AppendChild(group);
            }

            var fileNode = this.CreateNode("Path", filePath);
            this.AppendAttribute(fileNode, "name", name);
            group.AppendChild(fileNode);
        }

       
        public void DeleteSnippetFile(String groupName, String name)
        {
            var groupNode = RootNode.SelectNodes(_codeSnippets + "/*").ToList().FirstOrDefault(
                    x => x.SelectSingleNode("@name").Value == groupName);

            if (groupNode == null)
            {
                return;
            }

            var fileNode = groupNode.SelectNodes("*").ToList().FirstOrDefault(
                    x => x.SelectSingleNode("@name").Value == name);

            if (fileNode == null)
            {
                return;
            }

            groupNode.RemoveChild(fileNode);

        }
    }*/
}