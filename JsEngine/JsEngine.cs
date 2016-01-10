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
        public JsEngineType CurrentType {get; set;}
        public JsEngineXMLRepository Repository { get {
            if (CurrentType == JsEngineType.MongoDB) {
                return this.MongoXMLRepository;
            }
            return null;
        } }

        public void InitRepository(JsEngineType type) {
            CurrentType = type;

            var xmlFile = type.ToString() + ".xml";

            if (type == JsEngineType.MongoDB) {
                this.MongoXMLRepository = new MongoXMLRepository();
                this.MongoXMLRepository.Init(xmlFile);
            }
        }
        
    }
}
