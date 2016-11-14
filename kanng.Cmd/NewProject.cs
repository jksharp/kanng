using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

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

            Directory.Move(path + "\\data\\sys\\", path + "\\data\\" + textBox1.Text.Trim() + "\\");
        }
    }
}
