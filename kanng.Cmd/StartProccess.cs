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

namespace kanng.Cmd
{
    public partial class StartProccess : Form
    {
        public StartProccess()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] strs = KanngHelper.ReadAllLines();

            string errorFiles = "";
            if (strs != null)
            {
                foreach (string str in strs)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        //跳过注释
                        if (!str.StartsWith("#"))
                        {
                            if (File.Exists(str))
                            {
                                Process.Start(str);
                            }
                            else
                            {
                                errorFiles += str + "\r\n";
                            }
                        }
                    }
                    Thread.Sleep(200);

                }
            }

            if (errorFiles.Length > 0)
            {
                MessageBox.Show("没有启动成功的文件:\r\n" + errorFiles);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KanngHelper.WriteFile(richTextBox1.Text);
        }

        private void StartProccess_Load(object sender, EventArgs e)
        {
            LoadUrl();
            richTextBox1.Text = KanngHelper.ReadAllText();

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = KanngHelper.ReadAllText();
        }

        private void tabPage1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
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
                    richTextBox1.Text += s[i] + "\r\n";
                }
            }
            KanngHelper.WriteFile(richTextBox1.Text);
        }


        private void 关于看门狗ToolStripMenuItem_Click(object sender, EventArgs e)
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

        public void LoadUrl() {

            List<UrlModel> urlModel = UrlXmlIO.ReadAllUrl();
            if (urlModel != null)
            {
                foreach (UrlModel model in urlModel)
                {
                    this.urlPatte1 = new UrlPatte();

                    urlPatte1.SetModel(model.guid,model.url,model.name);
                    // 
                    // urlPatte1
                    // 
                    this.urlPatte1.Cursor = System.Windows.Forms.Cursors.Hand;
                    this.urlPatte1.Location = new System.Drawing.Point(3, 3);
                    this.urlPatte1.Name = "urlPatte1";
                    this.urlPatte1.Size = new System.Drawing.Size(87, 51);
                    this.urlPatte1.TabIndex = 3;

                    this.flowLayoutPanel1.Controls.Add(this.urlPatte1);                   
                  
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CreatUrl create = new CreatUrl();
            create.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.kanng.net");
        }

        public void addControl(UrlModel model) {
            //this.Refresh();
            this.urlPatte1 = new UrlPatte();

            urlPatte1.SetModel(model.guid, model.url, model.name);
            // 
            // urlPatte1
            // 
            this.urlPatte1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.urlPatte1.Location = new System.Drawing.Point(3, 3);
            this.urlPatte1.Name = "urlPatte1";
            this.urlPatte1.Size = new System.Drawing.Size(87, 51);
            this.urlPatte1.TabIndex = 3;
            this.flowLayoutPanel1.Controls.Add(this.urlPatte1);  
           
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }
       
    }
}
