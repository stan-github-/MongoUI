using DBUI.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBUI.JsEngine
{
    public class JsEngine<T> where T: JsEngineXMLRepository, new()
    {
        
        public JsEngineType Type
        {
            get
            {
                return Repository.Type;
            }
        }

        public T Repository { get ; set ; }

        public void InitRepository() {
            this.Repository = new T();
            var xmlFile = Type.ToString() + ".xml";
            this.Repository.Init(xmlFile);
            
        }
        
    }
}
