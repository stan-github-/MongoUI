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
        
        private const String _JsEngine = "Engines";
        
        private String _xmlPath;

        public bool Init() {
            _xmlPath = Application.StartupPath + "/main.xml";
            return base.Init(_xmlPath, "DocumentElement");
        }
        
        public class Engine{
            public string Name { get; set; }
            public bool IsCurrent { get; set; }
        }

        public List<Engine> Engines
        {
            get
            {
                var nodes = RootNode.SelectNodes(_JsEngine + "/*")
                    .ToList().Select(n => new Engine()
                    {
                        Name =  n.SelectSingleNode("Engine").InnerText,
                        IsCurrent = bool.Parse(n.SelectSingleNode("@IsCurrent").InnerText)
                    }).ToList();

                if (nodes == null)
                {
                    return new List<Engine>();
                }

                return nodes;
            }
        }
    }
}
