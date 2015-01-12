using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DBUI
{

    public static class XMLExtensions
    {
        public static String GetAttributeValue(this XElement e, String name)
        {
            var att = e.Attributes().FirstOrDefault(a => a.Name == name);
            return att == null ? null : att.Value;
        }

        public static XAttribute GetAttribute(this XElement e, String name)
        {
            return e.Attributes().FirstOrDefault(a => a.Name == name);
        }

        public static List<XmlNode> ToList(this XmlNodeList xmlNodelist)
        {
            if (xmlNodelist == null || xmlNodelist.Count == 0)
            {
                return new List<XmlNode>();
            }

            var l = new List<XmlNode>();
            foreach (XmlNode node in xmlNodelist)
            {
                l.Add(node);
            }
            return l;
        }
    }

    //class XMLManagerV2
    //{

    //    public XDocument _domDoc;
    //    private String _xmlFileName;

    //    public XElement RootNode { get; private set; }
    //    public XPathNavigator Navigator { get; private set; }
    //    /*
    //    public bool Init(ref string xmlString, string rootName)
    //    {
    //        try
    //        {
    //            _domDoc = XDocument.Load(xmlString);
    //            RootNode = _domDoc.Root;
    //            if (RootNode == null) { return false; }

    //        }
    //        catch (Exception ex) { this.ShowError(ex); }
    //        return true;
    //    }*/

    //    public bool Init(string xmlFileName, string rootName)
    //    {
    //        _xmlFileName = xmlFileName;
    //        return LoadXmlFromFile(rootName);
    //    }

    //    private bool LoadXmlFromFile(string rootName)
    //    {
    //        if (System.IO.File.Exists(_xmlFileName) == false)
    //        {
    //            this.ShowError("file: " + _xmlFileName + " not found");
    //            return false;
    //        }

    //        try
    //        {
    //            _domDoc = XDocument.Load(_xmlFileName);
    //            RootNode = _domDoc.Root;
    //            Navigator = RootNode.CreateNavigator();
    //            if (RootNode == null) { return false; }
    //        }
    //        catch (Exception ex)
    //        {
    //            this.ShowError(ex);
    //        }
    //        return true;
    //    }

    //    public bool SaveXml(string fileName)
    //    {
    //        try
    //        {
    //            _domDoc.Save(fileName);
    //        }
    //        catch (Exception ex)
    //        {
    //            this.ShowError(ex);
    //        }
    //        return true;
    //    }

    //    public bool SaveXml()
    //    {
    //        SaveXml(_xmlFileName);
    //        return true;
    //    }

    //    /*

    //    public XmlNode AppendNode(ref XmlNode parentNode, string nodeName, string nodeValue)
    //    {
    //        if (parentNode == null) { return null; }
    //        XmlNode n = this.CreateNode(nodeName, nodeValue);
    //        parentNode.AppendChild(n);
    //        return n;
    //    }

    //    public XmlNode CreateNode(string nodeName, string nodeValue)
    //    {
    //        XmlNode node = _domDoc.(XmlNodeType.Element, nodeName, "");
    //        node.InnerXml = nodeValue;
    //        return node;
    //    }

    //    public XmlNode AppendAttribute
    //        (ref XmlNode node, string attributeName, string attributeValue)
    //    {
    //        if (node == null) { return null; }
    //        XmlAttribute a = _domDoc.CreateAttribute(attributeName);
    //        a.InnerText = attributeValue;
    //        if (node.Attributes != null) node.Attributes.Append(a);
    //        return a;
    //    }*/

    //    #region error handling
    //    private void ShowError(string errorMessage)
    //    {
    //        MessageBox.Show(errorMessage);
    //    }

    //    private void ShowError(Exception ex)
    //    {
    //        MessageBox.Show(ex.ToString());
    //    }
    //    #endregion
    //}

    public class XMLManager{

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

        //should be made in into 
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
