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
        
        private const String _JsEngine = "JsEngines";
        
        private String _xmlPath;

        public bool Init() {
            _xmlPath = Application.StartupPath + "/main.xml";
            return base.Init(_xmlPath, "DocumentElement");
        }
        
        
        public List<DataModel.JsEngine> Engines
        {
            get
            {
                var nodes = RootNode.SelectNodes(_JsEngine + "/*")
                    .ToList().Select(n => new DataModel.JsEngine()
                    {
                        Type = (JsEngineType)Enum.Parse
                            (typeof(JsEngineType), 
                            n.SelectSingleNode("@name").InnerText),
                        IsCurrent = bool.Parse(n.SelectSingleNode("@isCurrent").InnerText)
                    }).ToList();

                if (nodes == null)
                {
                    return new List<DataModel.JsEngine>();
                }

                return nodes;
            }
        }

        public JsEngineType CurrentEngine {
            get {
                var n =  Program.MainXMLManager.Engines
                    .First(e => e.IsCurrent == true);
                return n.Type;
            }

            set {
                 foreach (XmlNode node in RootNode.SelectNodes(_JsEngine + "/*")){
                     var b = node.SelectSingleNode("@isCurrent");
                     b.InnerText = "false";

                     if (node.SelectSingleNode("@name").InnerText == value.ToString()){
                         b.InnerText = "true";
                     }
                 }
            }
        }
    }
}
