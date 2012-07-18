using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace DBUI
{
    class XMLManager{

        private XmlDocument _domDoc;
        private String _xmlFileName;

        public XmlNode RootNode { get; private set; }
        
        public bool Init(ref string xmlString, string rootName) {
            try {
                _domDoc = new XmlDocument();
                _domDoc.LoadXml(xmlString);
                RootNode = this._domDoc.SelectSingleNode(rootName);
                if (RootNode == null) {return false;}

            } catch (Exception ex) { this.ShowError(ex); }
            return true;
        }

        public bool Init(string xmlFileName, string rootName) {
            _xmlFileName = xmlFileName;
            _domDoc = new XmlDocument();
            
            return LoadXmlFromFile(rootName);
        }

        private bool LoadXmlFromFile(string rootName){
            if (System.IO.File.Exists(_xmlFileName) == false) {
                this.ShowError("file: " + _xmlFileName + " not found");
                return false;
            }

            try {
                _domDoc.Load(_xmlFileName);
                RootNode = _domDoc.SelectSingleNode(rootName);
                if (RootNode == null) { return false; }
            } catch (Exception ex) {
                this.ShowError(ex);
            }
            return true;    
        }
        
        public bool SaveXml(string fileName){
            try {
                _domDoc.Save(fileName);
            } catch (Exception ex) {
                this.ShowError(ex);
            }
            return true;    
        }

        public bool SaveXml() {
            SaveXml(_xmlFileName);
            return true;
        }


        public XmlNode AppendNode(ref XmlNode parentNode,
            string nodeName, string nodeValue) {
            if (parentNode == null) { return null; }
            XmlNode n = this.CreateNode(nodeName, nodeValue);
            parentNode.AppendChild(n);
            return n;
        }

        public XmlNode CreateNode(string nodeName, string nodeValue) {
            XmlNode node = _domDoc.CreateNode(XmlNodeType.Element,
                nodeName, "");
            node.InnerXml = nodeValue;
            return node;
        }

        public XmlNode AppendAttribute
            (ref XmlNode node, string attributeName, string attributeValue){
            if (node == null){return null;}
            XmlAttribute a = _domDoc.CreateAttribute(attributeName);
            a.InnerText = attributeValue;
            if (node.Attributes != null) node.Attributes.Append(a);
            return a;
        }
 
        #region error handling
        private void ShowError(string errorMessage) {
            MessageBox.Show(errorMessage);
        }

        private void ShowError(Exception ex) {
            MessageBox.Show(ex.ToString());
        }
        #endregion
    }
}
