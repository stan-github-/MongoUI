using DBUI.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBUI.JsEngine
{
    public class JsEngine
    {
        public MongoXMLRepository MongoXMLRepository;
        public NodeXMLRepository NodeXMLRepository;

        public JsEngineType CurrentType
        {
            get
            {
                return Program.MainXMLManager.CurrentEngine;
            }
        }

        public JsEngineXMLRepository Repository { get {
            if (CurrentType == JsEngineType.MongoDB) {
                return this.MongoXMLRepository;
            }
            else if (CurrentType == JsEngineType.Node) {
                return this.NodeXMLRepository;
            }
            return null;
            } 
        }

        public void InitRepository() {
            
            var xmlFile = CurrentType.ToString() + ".xml";

            if (CurrentType == JsEngineType.MongoDB)
            {
                this.MongoXMLRepository = new MongoXMLRepository();
                this.MongoXMLRepository.Init(xmlFile);
            }
            else if (CurrentType == JsEngineType.Node)
            {
                this.NodeXMLRepository = new NodeXMLRepository();
                this.NodeXMLRepository.Init(xmlFile);
            }
        }
        
    }
}
