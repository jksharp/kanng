using Kanng.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Kanng.Common
{
    public class UrlXmlIO
    {
        public static string xmlFileName = "\\data\\url\\Url.xml";

        private static int index = 0;

        static UrlXmlIO()
        {
            xmlFileName = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + xmlFileName;
        }


        public static bool Create(string url,string guid)
        {

            string xpath = "/root";  //这是新节点的父节点路径
            string Name = "url";
            string idname = "guid";
            StringBuilder sb = new StringBuilder();
            sb.Append("<name>"+url+"</name>");
            sb.Append("<url>"+url+"</url>");
            sb.Append("<date>"+DateTime.Now.ToShortTimeString()+"</date>");
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
                model.guid = node.Attributes[0].Value;
                model.name = node.ChildNodes[0].InnerText;
                model.url = node.ChildNodes[1].InnerText;
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
