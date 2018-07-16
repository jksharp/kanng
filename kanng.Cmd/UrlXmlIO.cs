using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace kanng.Cmd
{
    public class UrlXmlIO
    {
        public static string xmlFileName = "\\data\\url\\Url.xml";

        private static int index = 0;

        static UrlXmlIO()
        {
            xmlFileName = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + xmlFileName;
        }
        
        public static bool Create(string name,string url,string guid, string username, string pwd)
        {

            string xpath = "/root";  //这是新节点的父节点路径
            string Name = "url";
            string idname = "guid";
            StringBuilder sb = new StringBuilder();
            sb.Append("<name>"+ name+ "</name>");
            sb.Append("<url>"+url+"</url>");
            sb.Append("<username>" + username + "</username>");
            sb.Append("<pwd>" + pwd + "</pwd>");
            sb.Append("<date>"+DateTime.Now.ToString()+"</date>");
            sb.Append("<order>1</order>");

            bool isSuccess = XMLHelper.CreateXmlNodeByXPath(xmlFileName, xpath, Name, sb.ToString(), idname,guid);
            return isSuccess;
        }

        public static bool CreateOrUpdate(string url)
        {
            
            string xpath = "/root/url";  //这是新节点的父节点路径
            string attributeName = "id";　//新属性名称,ISDN号也是这么新增的
            string attributeValue = "1";　//新属性值

            bool isSuccess = XMLHelper.CreateOrUpdateXmlAttributeByXPath(xmlFileName, xpath, attributeName, attributeValue);
            return isSuccess;
        }

        public static bool UpdateNode(string name, string url, string guid,string username,string pwd)
        {     
            string xpath = "/root/url[@guid='" + guid + "']"; //要删除的id为1的book子节点
            string Name = "url";
            StringBuilder sb = new StringBuilder();
            sb.Append("<name>" + name + "</name>");
            sb.Append("<url>" + url + "</url>");
            sb.Append("<username>" + username + "</username>");
            sb.Append("<pwd>" + pwd + "</pwd>");
            sb.Append("<date>" + DateTime.Now.ToString() + "</date>");
            sb.Append("<order>1</order>");


            bool isSuccess = XMLHelper.CreateOrUpdateXmlNodeByXPath2(xmlFileName, xpath, Name, sb.ToString());
            return isSuccess;
        }


        public static List<UrlModel> ReadAllUrl()
        {
          
            //要读的id为1的book子节点
            string xpath = "/root/url";

            XmlNodeList nodeList = XMLHelper.GetXmlNodeListByXpath(xmlFileName, xpath);
            string strAllNode = "";
            List<UrlModel> list = new List<UrlModel>();
            //遍历节点中所有的子节点
            foreach (XmlNode node in nodeList)
            {
                UrlModel model = new UrlModel();
                XmlElement element = (XmlElement)node;
                model.guid = node.Attributes[0].Value;
                
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    if (node2.Name == "name")
                    {
                        model.name = element.GetElementsByTagName("name").Item(0).InnerText;
                    }
                    else if (node2.Name == "url")
                    {
                        model.url = element.GetElementsByTagName("url").Item(0).InnerText;
                    }
                    else if (node2.Name == "username")
                    {
                        model.username = element.GetElementsByTagName("username").Item(0).InnerText;
                    }
                    else if (node2.Name == "password")
                    {
                        model.password = element.GetElementsByTagName("password").Item(0).InnerText;
                    }

                }
             
                
                list.Add(model);
            }

            return list;
        }

        public static bool DeleteNode(string guid) {
            string xpath = "/root/url[@guid='" + guid + "']"; //要删除的id为1的book子节点

            bool isSuccess = XMLHelper.DeleteXmlNodeByXPath(xmlFileName, xpath);

            return isSuccess;
           
        }
    }
}
