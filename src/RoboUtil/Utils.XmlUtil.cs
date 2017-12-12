using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RoboUtil
{
    partial class Utils
    {
        public static class XmlUtil
        {
            public static XmlDocument ToXmlDocument(XDocument xDocument)
            {
                var xmlDocument = new XmlDocument();
                using (var xmlReader = xDocument.CreateReader())
                {
                    xmlDocument.Load(xmlReader);
                }
                return xmlDocument;
            }

            public static XDocument ToXDocument(XmlDocument xmlDocument)
            {
                using (var nodeReader = new XmlNodeReader(xmlDocument))
                {
                    nodeReader.MoveToContent();
                    return XDocument.Load(nodeReader);
                }
            }

            public static XmlDocument ToXmlDocument(XElement xElement)
            {
                var sb = new StringBuilder();
                var xws = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false };
                using (var xw = XmlWriter.Create(sb, xws))
                {
                    xElement.WriteTo(xw);
                }
                var doc = new XmlDocument();
                doc.LoadXml(sb.ToString());
                return doc;
            }

            public static Stream ToMemoryStream(XmlDocument doc)
            {
                var xmlStream = new MemoryStream();
                doc.Save(xmlStream);
                xmlStream.Flush();//Adjust this if you want read your data
                xmlStream.Position = 0;
                return xmlStream;
            }

            /// <summary>
            /// this function select rootnode in xml after read all childs
            /// you must define 2 attribute in childs, first is name and second is Value
            /// </summary>
            /// <param name="rootNodeName">root node name for namevalue child nodes</param>
            /// <param name="xmlPath">xml file path or file name </param>
            /// <returns>NameValueCollection</returns>
            public static NameValueCollection ReadNameValueXml(string rootNodeName, string xmlPath)
            {
                /* Example xml file
                 <?xml version="1.0" encoding="utf-8" standalone="yes"?>
                    <configurations>
                        <config key="testKey" value="testValue"/>
                    </configurations>

                 */
                NameValueCollection nameValueCollection = new NameValueCollection();
                string path = FileUtil.TryFixFilePath(xmlPath);

                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNode node = doc.SelectSingleNode("//" + rootNodeName);
                    foreach (XmlNode item in node.ChildNodes)
                    {
                        if (item.NodeType != XmlNodeType.Comment)
                        {
                            string key = item.Attributes[0].Value;
                            string val = item.Attributes[1].Value;
                            nameValueCollection.Add(key, val);
                        }
                    }
                }
                catch (System.IO.FileNotFoundException e)
                {
                    throw new Exception("Xml File not found! or Xml file can not read!", e);
                }
                return nameValueCollection;
            }
        }
    }
}