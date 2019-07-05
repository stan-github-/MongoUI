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
        
        public JsEngineXMLRepository Repository {
            get
            {
                return MongoEngine.Repository;                
            }
        }

        public JsEngineProxy() {
            
            MongoEngine = new JsEngine.JsEngine<MongoXMLRepository>();
            MongoEngine.InitRepository();
            
        }

        public MongoXMLRepository GetMongoRepo()
        {
            return this.MongoEngine.Repository;
        }

        //todo when to reload the repor
    }
}
