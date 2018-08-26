using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kanng.Common
{
	public class RB
	{
		public string hbdx { get; set; }
		public string rq { get; set; }
		public string gznr { get; set; }
		public string ydwt { get; set; }
		public string mtjh { get; set; }
		public string xz { get; set; }
		public string name { get; set; }
		public string bm { get; set; }
	}
	public class NPOIWordHelper
	{

		public static string FilePath = "\\data\\DayReport\\tt.docx";

		public static string SaveFileName = "个人工作日报-{0}-{1}";

		static NPOIWordHelper()
		{
			FilePath = System.AppDomain.CurrentDomain.BaseDirectory  + FilePath;
		}


		public static void Export(RB rb)
		{			
			using (FileStream stream = File.OpenRead(FilePath))
			{
				XWPFDocument doc = new XWPFDocument(stream);
				//遍历段落                  
				foreach (var para in doc.Paragraphs)
				{
					ReplaceKey(para, rb);
				}                    //遍历表格      
				var tables = doc.Tables;
				foreach (var table in tables)
				{
					foreach (var row in table.Rows)
					{
						foreach (var cell in row.GetTableCells())
						{
							foreach (var para in cell.Paragraphs)
							{
								ReplaceKey(para, rb);
							}
						}
					}
				}

				FileStream out1 = new FileStream(Path.GetDirectoryName(FilePath)+"\\" +string.Format(SaveFileName,rb.name,DateTime.Now.ToString("yyyy-MM-dd"))+"_" + DateTime.Now.Ticks + ".docx", FileMode.Create);
				doc.Write(out1);
				out1.Close();
			}
		}
		private static void ReplaceKey(XWPFParagraph para, object model)
		{
			string text = para.ParagraphText;
			var runs = para.Runs;
			string styleid = para.Style;
			for (int i = 0; i < runs.Count; i++)
			{
				var run = runs[i];
				text = run.ToString();
				Type t = model.GetType();
				PropertyInfo[] pi = t.GetProperties();
				foreach (PropertyInfo p in pi)
				{

					//$$与模板中$$对应，也可以改成其它符号，比如{$name},务必做到唯一
					if (text.Contains("$" + p.Name + "$"))
					{
						text = text.Replace("$" + p.Name + "$", p.GetValue(model, null).ToString());
					}
				}
				runs[i].SetText(text, 0);
			}
		}
	}
}
