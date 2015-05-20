using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kanng.Cmd
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        StartProccess newForm = null;

        leftmenu leftform = null;
        private void main_Activated(object sender, EventArgs e)
        {
            newForm = new StartProccess();
            newForm.Show();

            leftform = new leftmenu();
            leftform.Show();

            this.Hide();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form newForm1 = CheckMdiFormIsOpen("StartProccess");
            if (newForm1 == null)
            {
                StartProccess from = new StartProccess();
                from.Show();
            }
            else
            {
                newForm1.WindowState = FormWindowState.Normal;
                newForm1.Show();
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public Form CheckMdiFormIsOpen(string asFormName)
        {
            Form form = null;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == asFormName)
                {
                    form = frm;
                    break;
                }
            }
            return form;
        }
    }
}
    