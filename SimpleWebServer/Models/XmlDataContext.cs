using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleWebServer.Models
{
    public class XmlDataContext<T> : IDataContext<T> where T : class, IDataItem
    {
        private string path;
        private XmlSerializer serializer;
        private string itemNodeName;

        public XmlDataContext(string path)
        {
            this.itemNodeName = typeof(T).Name;
            this.path = path;
            this.serializer = new XmlSerializer(typeof(T));
            if (!File.Exists(path))
            {
                CreateDocument(path);
            }
        }
        public T GetById(int id)
        {
            T item = null;
            XmlDocument document = LoadDocument();
            XmlNode node = document.SelectSingleNode("//" + itemNodeName + "[@id='" + id + "']");
            if (node != null)
            {
                item = DeserializeItem(node);
            }
            return item;
        }
        public IEnumerable<T> GetAll()
        {
            List<T> items = new List<T>();
            XmlDocument document = LoadDocument();
            XmlNodeList nodes = document.SelectNodes("//" + itemNodeName);

            return nodes.Cast<XmlNode>().Select(x => DeserializeItem(x));
        }
        public void Add(T item)
        {
            XmlDocument document = LoadDocument();
            
            int id = 0;
            XmlNode lastIdNode = document.SelectSingleNode("//" + itemNodeName + "[last()]/Id");
            if (lastIdNode != null && int.TryParse(lastIdNode.InnerText, out id))
                id++;
            item.Id = id;
            item.DateCreated = DateTime.Now;
            XmlNode node = SereatizeItem(item);
            
            document.SelectSingleNode("//DataItems").AppendChild(document.ImportNode(node, true));
            document.Save(path);
        }

        private XmlDocument CreateDocument(string path)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode dataItemsNode = doc.CreateElement("DataItems");
            doc.AppendChild(dataItemsNode);
            doc.Save(path);
            return doc;
        }

        private XmlDocument LoadDocument()
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);
            return document;
        }

        private T DeserializeItem(XmlNode node)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter streamWriter = new StreamWriter(stream);
                streamWriter.Write(node.OuterXml);
                streamWriter.Flush();

                stream.Position = 0;
                T result = (serializer.Deserialize(stream) as T);

                return result;
            }
        }

        private XmlNode SereatizeItem(T item)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, item);
                memoryStream.Position = 0;
                XmlDocument doc = new XmlDocument();
                doc.Load(memoryStream);
                return doc.DocumentElement;
            }
        }
    }
}
