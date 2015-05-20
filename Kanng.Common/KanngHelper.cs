using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kanng.Common
{
    public class KanngHelper
    {
        public static string FilePath = "\\data\\kanng.data";

        static KanngHelper() {
            FilePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + FilePath;
        }

        public static void WriteFile(string data) {
       

            File.WriteAllText(FilePath, data);
            
        }

        public static string[] ReadAllLines()
        {
            if (!File.Exists(FilePath)) return null;
            return File.ReadAllLines(FilePath); 
        }

        public static string ReadAllText()
        {
            if (!File.Exists(FilePath)) return "";
            return File.ReadAllText(FilePath);
        }
    }
}
