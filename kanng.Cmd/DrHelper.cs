using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace kanng.Cmd
{
    public class DrHelper
	{

        // public static string FilePath = "\\data\\kanng.data";
        public static string HostsFilePath = @"C:\Windows\System32\drivers\etc\hosts";
        public static string sFilePath = "data\\DayReport\\kanng.data";
        public static string FilePath = "";


		DrHelper()
        {
            FilePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + sFilePath;
        }


        public static DrHelper Single()
        {
            DrHelper single = new DrHelper();

            return single;
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


    }



}
