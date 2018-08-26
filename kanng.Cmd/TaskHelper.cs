using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace kanng.Cmd
{
	public class TaskHelper
	{

		// public static string FilePath = "\\data\\kanng.data";
		public static string HostsFilePath = @"C:\Windows\System32\drivers\etc\hosts";
		public static string FilePath = "data\\task\\kanng.data";


		TaskHelper()
		{
			FilePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + FilePath;
		}


		public static TaskHelper Single()
		{
			TaskHelper single = new TaskHelper();
		
			return single;
		}


		public TaskHelper(string path)
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

	
	}



}
