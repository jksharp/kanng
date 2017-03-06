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
        public NewProject()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = Environment.CurrentDirectory;
            
            DirectoryIO.CopyFolder(path + "\\data\\sys\\", path + "\\data\\" + textBox1.Text.Trim() + "\\");

            MessageBox.Show(textBox1.Text.Trim()+"创建成功，请重新打开看门狗桌面启动助手！");
        }
    }
}
