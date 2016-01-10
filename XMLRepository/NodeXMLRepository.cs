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
   
    public class NodeXMLRepository : JsEngineXMLRepository {
        
        public bool Init() {
            return base.Init(JsEngineType.Node);
        }
        
    }
}
