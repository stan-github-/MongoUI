using DBUI.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBUI.JsEngine
{
    public class JsEngineProxy
    {
        public JsEngine.JsEngine<MongoXMLRepository> MongoEngine;
        public JsEngine.JsEngine<NodeXMLRepository> NodeEngine;
        
        public JsEngine<JsEngineXMLRepository> CurrentEngine
        {
            get
            {
                if (Program.MainXMLManager.CurrentEngine == JsEngineType.MongoDB)
                {
                    //this has to be an interface
                    return (JsEngine<JsEngineXMLRepository>)(Object)MongoEngine;
                }
                else if (Program.MainXMLManager.CurrentEngine == JsEngineType.Node)
                {
                    return (JsEngine<JsEngineXMLRepository>)(Object)NodeEngine;
                }

                return null;
            }
        }

        public JsEngineXMLRepository Repository {
            get
            {
                if (Program.MainXMLManager.CurrentEngine == JsEngineType.MongoDB)
                {
                    //this has to be an interface
                    return MongoEngine.Repository;
                }
                else if (Program.MainXMLManager.CurrentEngine == JsEngineType.Node)
                {
                    return NodeEngine.Repository;
                }

                return null;
            }
        }

        public JsEngineProxy() {
            
            MongoEngine = new JsEngine.JsEngine<MongoXMLRepository>();
            MongoEngine.InitRepository();

            NodeEngine = new JsEngine.JsEngine<NodeXMLRepository>();
            NodeEngine.InitRepository();
            
        }

        //todo when to reload the repor
    }
}
