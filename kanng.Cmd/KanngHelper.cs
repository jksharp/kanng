using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace kanng.Cmd
{
    public class KanngHelper
    {
        // public static string FilePath = "\\data\\kanng.data";
        public static string HostsFilePath = @"C:\Windows\System32\drivers\etc\hosts";
        public static string FilePath = "";


        static KanngHelper()
        {

            FilePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + FilePath;
        }



        public static KanngHelper SingleKanng(string path)
        {
            KanngHelper single = new KanngHelper(FilePath);
            FilePath = System.AppDomain.CurrentDomain.BaseDirectory + "data\\" + path + "\\" + "\\data\\kanng.data";
            return single;
        }


        public KanngHelper(string path)
        {
            FilePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + path + "\\" + FilePath;
        }

        public void WriteFile(string data)
        {


            File.WriteAllText(FilePath, data);

        }

        public string[] ReadAllLines()
        {
            if (!File.Exists(FilePath)) return null;
            return File.ReadAllLines(FilePath);
        }

        public string ReadAllText()
        {
            if (!File.Exists(FilePath)) return "";
            return File.ReadAllText(FilePath);
        }



        public void WriteHostsFile(string data)
        {
            File.WriteAllText(HostsFilePath, data);
        }

        public string[] ReadHostsAllLines()
        {
            if (!File.Exists(HostsFilePath)) return null;
            return File.ReadAllLines(HostsFilePath);
        }

        public string ReadHostsAllText()
        {
            if (!File.Exists(HostsFilePath)) return "";
            return File.ReadAllText(HostsFilePath);
        }




    }
}
