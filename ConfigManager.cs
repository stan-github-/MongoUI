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

    public class ConfigManager
    {
        public ConfigModel Data { get; set; }
        public string FilePath { get; set; }

        public ConfigManager(string path)
        {
            FilePath = path;
            Load(path);
        }

        public void Reload() {
            Load(this.FilePath);
        
        }
        public void Load(string path) {
            var str = File.ReadAllText(path);
            Data = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigModel>(str);
        }

        public void Save() {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(Data);
            File.WriteAllText(FilePath, str);
        }

        public Server CurrentServer(string serverAlias = null) {
            if (serverAlias == null) {
                return Data.Servers.Find(s => s.IsCurrent);
            }

            Data.Servers.ForEach(s => s.IsCurrent = false);

            var server = Data.Servers.First(s => s.Alias == serverAlias);
            server.IsCurrent = true;
            return server;
        }

        public string LastOpenedFile() {
            var last = Data.Miscellaneous.LastOpenedFilePaths.Last();
            return last;
        }

        public OutputType CurrentQueryOutputType(){
            var type = Data.QueryOutputTypes.First(q => q.IsCurrent);
            return type;
        }
    }
}

        