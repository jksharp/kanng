using System;
using System.Xml.Serialization;


namespace Kanng.SyntaxTextBox
{
    public class Common
	{

        public static System.Enum GetEnum(System.Type enumType, string name)
        {
            string[] arr = System.Enum.GetNames(enumType);
            foreach (object o in System.Enum.GetValues(enumType))
            {
                if (System.Enum.GetName(enumType, o) == name)
                    return (System.Enum)o;
            }
            return null;
        }


        // XML–Ú¡–ªØ¥Ê»°-------------------------------------------------------------
        public static object LoadXml(string filePath, System.Type type)
        {
            if (!System.IO.File.Exists(filePath))
                throw new Exception("Can't find the xml file");
            using (System.IO.StreamReader reader = new System.IO.StreamReader(filePath))
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(type);
                object obj = xs.Deserialize(reader);
                reader.Close();
                return obj;
            }
        }

        public static void SaveXml(string filePath, object obj) {SaveXml(filePath, obj, obj.GetType());}
        public static void SaveXml(string filePath, object obj, System.Type type)
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath))
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(type);
                xs.Serialize(writer, obj);
                writer.Close();
            }
        }
	}
}
