using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace kanng.Cmd
{
    public class KanngHelper
    {
        public static string FilePath = "\\data\\kanng.data";
        public static string HostsFilePath = @"C:\Windows\System32\drivers\etc\hosts";

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



        public static void WriteHostsFile(string data)
        {
            File.WriteAllText(HostsFilePath, data);
        }

        public static string[] ReadHostsAllLines()
        {
            if (!File.Exists(HostsFilePath)) return null;
            return File.ReadAllLines(HostsFilePath);
        }

        public static string ReadHostsAllText()
        {
            if (!File.Exists(HostsFilePath)) return "";
            return File.ReadAllText(HostsFilePath);
        }




    }
}
