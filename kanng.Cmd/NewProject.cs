using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Kanng.Common;

namespace kanng.Cmd
{
    public partial class NewProject : Form
    {

        public int ProjectId = 1;

        /// <summary>
        /// 1表示新建2表示重命名
        /// </summary>
        public int FormMode = 1;




        public NewProject()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (FormMode == 1)
            {
                string path = Environment.CurrentDirectory;

                DirectoryIO.CopyFolder(path + "\\data\\sys\\", path + "\\data\\" + textBox1.Text.Trim() + "\\");

                MessageBox.Show(textBox1.Text.Trim() + "创建成功，请重新打开看门狗桌面启动助手！");
            }
            else if (FormMode == 2)
            {
                textBox1.Text = "当前项目";

                string path = Environment.CurrentDirectory;

                DirectoryIO.CopyFolder(path + "\\data\\sys\\", path + "\\data\\" + textBox1.Text.Trim() + "\\");

                MessageBox.Show(textBox1.Text.Trim() + "修改成功，请重新打开看门狗桌面启动助手！");
            }
        }
    }
}
