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

    public class PhantomJsXMLRepository : XMLManager {
        
         
        private const String _WrapperJsFilePaths = "PhantomJs/WrapperJsFilePath";
        private const String _ExeFilePath = "PhantomJs/ExeFilePath";
        private const String _customJSFilePaths = "PhantomJs/CustomJavascriptFiles/Path";

        private String _xmlPath;

        public bool Init() {
            _xmlPath = Application.StartupPath + "/MongoXML.xml";
            return base.Init(_xmlPath, "DocumentElement");
        }

        public List<String> CustomJSFilePaths
        {
            get
            {
                List<String> sl = new List<string>();
                XmlNodeList ns = RootNode.SelectNodes(_customJSFilePaths);
                foreach (XmlNode n in ns)
                {
                    sl.Add(n.InnerText);
                }
                return sl;
            }
        }
        

        public String WrapperJsFilePaths
        {
            set
            {
                XmlNode n = RootNode.SelectSingleNode(_WrapperJsFilePaths);
                if (n!=null) {n.InnerText = value;}
            }
            get
            {
                XmlNode n = RootNode.SelectSingleNode(_WrapperJsFilePaths);
                if (n != null)
                {
                    return n.InnerText;
                }
                return null;
            }

        }

        public String ExeFilePath
        {
            set
            {
                XmlNode n = RootNode.SelectSingleNode(_ExeFilePath);
                if (n != null) { n.InnerText = value; }
            }
            get
            {
                XmlNode n = RootNode.SelectSingleNode(_ExeFilePath);
                if (n != null)
                {
                    return n.InnerText;
                }
                return null;
            }

        }
    }
}
