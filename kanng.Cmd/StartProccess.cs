using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.Devices;
using Kanng.Common;

namespace kanng.Cmd
{
    public partial class StartProccess : Form
    {
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 加载的文件路径
        /// </summary>
        public string LoadPath
        {
            get;
            set;
        }

		/// <summary>
		/// 加载的任务文件路径
		/// </summary>
		public string LoadTaskPath
		{
			get;
			set;
		}

		public int Limetime
        {
            get;
            set;
        }

        /// <summary>
        /// 文件扩展名 类型 文件 可以分为  可以执行文件 和 非 执行文件  
        /// </summary>
        int pathAttr = 0;

        public StartProccess(string path)
        {
            LoadPath = path;

            Title = "安雀桌面助手1.4-" + path;

            InitializeComponent();

            syntaxTextBox1.AllowDrop = true;

            syntaxTextBox1.DragEnter += new DragEventHandler(richTextBox1_DragEnter);

            syntaxTextBox1.DragDrop += new DragEventHandler(richTextBox1_DragDrop);
        }

        private void richTextBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void richTextBox1_DragDrop(object sender, DragEventArgs e)
        {
            Array arrarFileName = (Array)e.Data.GetData(DataFormats.FileDrop);
            string strFileName = arrarFileName.GetValue(0).ToString();
            
            syntaxTextBox1.Text += strFileName + "\r\n";

            KanngHelper.SingleKanng(LoadPath).WriteFile(syntaxTextBox1.Text);

        }


        private void StartProccess_Load(object sender, EventArgs e)
        {
            this.Text = Title;
            Limetime = 1000;


			// LoadUrl();  
		}

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //richTextBox1.Text = KanngHelper.ReadAllText();

        }

        private void tabPage1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void tabPage1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i;
            if (s != null)
            {
                for (i = 0; i < s.Length; i++)
                {
                    syntaxTextBox1.Text += s[i] + "\r\n";
                }
            }
            KanngHelper.SingleKanng(LoadPath).WriteFile(syntaxTextBox1.Text);
        }


        private void 关于安雀ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about aboutform = new about();
            aboutform.Show();
        }

        private void 赞助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jz aboutform = new jz();
            aboutform.Show();
        }
        private UrlPatte urlPatte1 = null;

        public void LoadUrl()
        {
            // Uri rulpaht = new Uri("http://www.FLYBI.CN");

            //webBrowser1.Url = rulpaht;

            syntaxTextBox1.Text = KanngHelper.SingleKanng(LoadPath).ReadAllText();
			syntaxTextBox2.Text = TaskHelper.Single().ReadAllText();
			ReadWorkReport();//绑定

			List<UrlModel> urlModel = UrlXmlIO.ReadAllUrl();

            if (urlModel != null)
            {
                foreach (UrlModel model in urlModel)
                {
                    this.urlPatte1 = new UrlPatte();

                    urlPatte1.SetModel(model.guid, model.url, model.name,model.username,model.password);
                    // 
                    // urlPatte1
                    // 
                    this.urlPatte1.Cursor = System.Windows.Forms.Cursors.Hand;
                    this.urlPatte1.Location = new System.Drawing.Point(3, 3);
                    this.urlPatte1.Name = model.guid;
                    this.urlPatte1.Size = new System.Drawing.Size(87, 51);
                    this.urlPatte1.TabIndex = 3;

                    this.flowLayoutPanel1.Controls.Add(this.urlPatte1);

                }
            }

        }

		public void ReadWorkReport() {

			string[] contents=DrHelper.Single().ReadAllLines();
			StringBuilder hqlc = new StringBuilder();
			if (contents != null)
			{
				int start = 1;
				foreach (var content in contents)
				{
					if (content == "##name s##")
					{
						start = 1;
						//name.Text = content;
						hqlc.Clear();
						continue;
					}
					if (content == "##name e##")
					{
						name.Text = hqlc.ToString();
						start = -1;
						continue;
					}
				

					if (content == "##bm s##")
					{
						start = 1;
						//name.Text = content;
						hqlc.Clear();
						continue;
					}
					if (content == "##bm e##")
					{
						bm.Text = hqlc.ToString();
						start = -1;
						continue;

					}
					

					if (content == "##hbdx s##")
					{
						start = 1;
						//name.Text = content;
						hqlc.Clear();
						continue;
					}
					if (content == "##hbdx e##")
					{
						hbdx.Text = hqlc.ToString();
						start = -1;
						continue;

					}
				

					if (content == "##gznr s##")
					{
						start = 1;
						//name.Text = content;
						hqlc.Clear();
						continue;
					}
					if (content == "##gznr e##")
					{
						gznr.Text = hqlc.ToString();
						start = -1;
						continue;
					}
					
					if (content == "##wt s##")
					{
						start = 1;
						//name.Text = content;
						hqlc.Clear();
						continue;
					}
					if (content == "##wt e##")
					{
						wt.Text = hqlc.ToString();
						start = -1;
						continue;
					}
				
				
				
					if (content == "##mtjh s##")
					{
						start = 1;
						//name.Text = content;
						hqlc.Clear();
						continue;
					}
					if (content == "##mtjh e##")
					{
						mtjh.Text = hqlc.ToString();
						start = -1;
						continue;

					}

					if (content == "##xz s##")
					{
						start = 1;
						//name.Text = content;
						hqlc.Clear();
						continue;
					}
					if (content == "##xz e##")
					{
						xz.Text = hqlc.ToString();
						start = -1;
						continue;
					}
					if (start == 1)
						hqlc.AppendLine(content);

					

				}
			}
			//StringBuilder hql = new StringBuilder();
			//hql.AppendLine(name.Text);
			//hql.AppendLine("");
			//hql.AppendLine(bm.Text);
			//hql.AppendLine("");
			//hql.AppendLine(bm.Text);
			//hql.AppendLine("");
			//hql.AppendLine(gznr.Text);
			//hql.AppendLine("");
			//hql.AppendLine(wt.Text);
			//hql.AppendLine("");
			//hql.AppendLine(hbdx.Text);
			//hql.AppendLine("");
			//hql.AppendLine(DateTime.Now.ToShortDateString());
			
		}

        private void button6_Click(object sender, EventArgs e)
        {
            CreatUrl create = new CreatUrl();
            create.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.FLYBI.CN");
        }

        public void addControl(UrlModel model)
        {
            //this.Refresh();
            this.urlPatte1 = new UrlPatte();

            urlPatte1.SetModel(model.guid, model.url, model.name, model.username, model.password);
            // 
            // urlPatte1
            // 
            this.urlPatte1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.urlPatte1.Location = new System.Drawing.Point(3, 3);
            this.urlPatte1.Name = model.guid;
            this.urlPatte1.Size = new System.Drawing.Size(87, 51);
            this.urlPatte1.TabIndex = 3;
            this.flowLayoutPanel1.Controls.Add(this.urlPatte1);

        }

        public void updateControl(UrlModel model)
        {
            //this.Refresh();
            this.urlPatte1 = (UrlPatte)this.flowLayoutPanel1.Controls.Find(model.guid, false)[0];

            urlPatte1.SetModel(model.guid, model.url, model.name, model.username, model.password);
            // 
            // urlPatte1
            // 
            this.urlPatte1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.urlPatte1.Location = new System.Drawing.Point(3, 3);
            this.urlPatte1.Name = model.guid;
            this.urlPatte1.Size = new System.Drawing.Size(87, 51);
            this.urlPatte1.TabIndex = 3;
            urlPatte1.label1.Text = model.name;
            this.urlPatte1.Refresh();




        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tabControl1.SelectedIndex == 2)
            {
                this.WindowState = FormWindowState.Maximized;

                richTextBox2.Text = File.ReadAllText(@"C:\Windows\System32\drivers\etc\hosts");
            }
            else if (tabControl1.SelectedIndex == 4)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<UrlModel> urlModel = UrlXmlIO.ReadAllUrl();
            if (urlModel != null)
            {
                foreach (UrlModel model in urlModel)
                {
                    if (model.url.ToLower().StartsWith("https") || model.url.ToLower().StartsWith("http"))
                        System.Diagnostics.Process.Start(model.url);
                    else
                        System.Diagnostics.Process.Start("http://" + model.url);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("mspaint.exe");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("regedit.exe");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                Save();
            }
        }


        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void iT导航ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.FLYBI.CN");
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();


        }


        public void Save()
        {
            new Thread(() =>
            {

                Action<string> action = (s) =>
                {
                    toolStripProgressBar1.Visible = true;
                    toolStripStatusLabel1.Text = s;
                    toolStripProgressBar1.Value = 0;
                    while (toolStripProgressBar1.Value < 100)
                    {

                        toolStripProgressBar1.PerformStep();
                    }

                };
                this.Invoke(action, LoadPath + "项目正在保存");
                Thread.Sleep(1000);
                this.Invoke(action, LoadPath + "项目保存完成");

            }).Start();


            if (tabControl1.SelectedIndex == 3)
            {
                TaskHelper.Single().WriteFile(syntaxTextBox2.Text);
            }
            else if (tabControl1.SelectedIndex == 2)
            {

                if (tabControl2.SelectedIndex == 1)

                    KanngHelper.SingleKanng(LoadPath).WriteHostsFile(richTextBox2.Text);

                else if (tabControl2.SelectedIndex == 2)

                    KanngHelper.SingleKanng(LoadPath).WriteHostsFile(richTextBox1.Text);
            }
            else
            {
                KanngHelper.SingleKanng(LoadPath).WriteFile(syntaxTextBox1.Text);
            }

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProject newp = new NewProject();
            newp.Show();
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helper newp = new Helper();
            newp.Show();
        }

        private void StartProccess_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                Save();
            }
        }


        private void richTextBox1_LinkClicked_1(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            StartFiles();
        }


        public void StartFiles()
        {


            string[] strs = KanngHelper.SingleKanng(LoadPath).ReadAllLines();

            bool ifcontine = true;
            bool rlb = false;
            string errorFiles = "";
            if (strs != null)
            {
                foreach (string str in strs)
                {


                    try
                    {

                        if (!string.IsNullOrEmpty(str))
                        {
                            if (str.Contains("/*"))
                            {
                                ifcontine = false;
                                continue;
                            }

                            if (str.Contains("*/"))
                            {
                                ifcontine = true;
                                continue;
                            }

                            if (!ifcontine) continue;

                            //跳过注释
                            if (!str.StartsWith("#"))
                            {

                                rlb |= true;
                                Process pro = Process.Start(str);

                                Thread.Sleep(Limetime);

                                //if (File.Exists(str) || Directory.Exists(str))
                                //{

                                //}
                                ////else if (Directory.Exists(str))
                                ////{
                                ////    rlb |= true;
                                ////    Process.Start("Explorer.exe", str);
                                ////    Thread.Sleep(Limetime);
                                ////}
                                //else
                                //{
                                //    errorFiles += str + "\r\n";
                                //}
                            }
                        }
                    }
                    catch (Exception ex)//容错处理
                    {
                        errorFiles += str + "\r\n";
                    }
                }
            }

            if (!rlb)
            {
                MessageBox.Show("没有需要启动的文件,您可以拖动文件到界面来添加文件!");
            }
            if (errorFiles.Length > 0)
            {
                MessageBox.Show("没有启动成功的文件:\r\n" + errorFiles);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //关闭现有进程


        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("mstsc.exe");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("inetmgr");
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl2.SelectedIndex == 2)
            {
                this.WindowState = FormWindowState.Maximized;

                richTextBox2.Text = File.ReadAllText(@"C:\Windows\System32\drivers\etc\hosts");
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;

                richTextBox1.Text = File.ReadAllText(@"C:\Windows\System32\drivers\etc\hosts");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            Save();

        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void richTextBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                Save();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
           string projectname=  LoadPath;
            NewProject newp = new NewProject();
            newp.FormMode = 2;//新建
            newp.ProjectName = projectname;
            newp.Show();
        }

        private void 运行ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

		private void toolStripSplitButton1_ButtonClick_1(object sender, EventArgs e)
		{

		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			toolStripStatusLabel3.Text = DateTime.Now.ToString();
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
	
			new Thread(() =>
			{

				Action<string> action = (s) =>
				{
					do
					{
						if (DateTime.Now.Hour > 9)
						{
							TimeSpan endtime = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
							double value = 9;
							double usertime = endtime.TotalSeconds * 1.0 / 60 / 60;
							int int_p = Convert.ToInt32(usertime * 100 / 9);

							toolStripProgressBar2.Value = int_p>0 && int_p < 100 ? int_p : 100;

							if (toolStripProgressBar2.Value > 20 && toolStripProgressBar2.Value < 50)
							{
								toolStripProgressBar2.BackColor = Color.Blue;
							}
							else if (toolStripProgressBar2.Value > 50)
							{
								toolStripProgressBar2.BackColor = Color.Orange;
							}
						}

					}
					while (1!=1);

				};
				this.Invoke(action,  "项目正在保存");
				Thread.Sleep(1000);
				

			}).Start();

		//	timer2.Stop();
		
			//Thread.Sleep(1000);
			//toolStripProgressBar2.Value = 0;
		}

		private void timer3_Tick(object sender, EventArgs e)
		{
			//toolStripProgressBar2.Value = 0;
		}

		private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{

		}

		private void button11_Click(object sender, EventArgs e)
		{

		}

		private void button11_Click_1(object sender, EventArgs e)
		{

			//rb.gznr = "建数据库";
			//rb.hbdx = "领导A";
			//rb.mtjh = "和业务部门A沟通业务";
			//rb.rq = DateTime.Now.ToShortDateString();
			//rb.xz = "需要业务部门协助";
			//rb.ydwt = "业务不明确";
			//rb.name = "";
			//rb.bm = "";

			RB rb = new RB();
			rb.gznr = gznr.Text.Trim();//工作内容
			rb.hbdx = hbdx.Text.Trim() ;//汇报对象
			rb.mtjh = mtjh.Text.Trim();//明天计划
			rb.rq = DateTime.Now.ToShortDateString();
			rb.xz = xz.Text.Trim();//是否需要协助协助
			rb.ydwt = wt.Text.Trim();//问题
			rb.name = name.Text.Trim();//名称
			rb.bm = bm.Text.Trim();//部门
			NPOIWordHelper.Export(rb);

			StringBuilder hql = new StringBuilder();
			hql.AppendLine("##name s##");
			hql.AppendLine(name.Text.Trim());
			hql.AppendLine("##name e##");

			hql.AppendLine("##bm s##");
			hql.AppendLine(bm.Text.Trim());
			hql.AppendLine("##bm e##");
			hql.AppendLine("##hbdx s##");
			hql.AppendLine(hbdx.Text.Trim());
			hql.AppendLine("##hbdx e##");
			hql.AppendLine("##gznr s##");
			hql.AppendLine(gznr.Text.Trim());
			hql.AppendLine("##gznr e##");
			hql.AppendLine("##wt s##");
			hql.AppendLine(wt.Text.Trim());
			hql.AppendLine("##wt e##");
			hql.AppendLine("##mtjh s##");
			hql.AppendLine(mtjh.Text.Trim());
			hql.AppendLine("##mtjh e##");
			hql.AppendLine("##xz s##");
			hql.AppendLine(xz.Text.Trim());
			hql.AppendLine("##xz e##");
			hql.AppendLine("##datetime s##");
			hql.AppendLine(DateTime.Now.ToShortDateString());
			hql.AppendLine("##datetime e##");
			DrHelper.Single().WriteFile(hql.ToString());
			
			MessageBox.Show("保存成功！");
		}
	}





}
