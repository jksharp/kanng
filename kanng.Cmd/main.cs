using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.Devices;
using System.IO;


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

            string localhost = Directory.GetCurrentDirectory() + "\\data\\";

            string[] dirs = Directory.GetDirectories(localhost);


            foreach (string str in dirs)
            {
               
                DirectoryInfo info = new DirectoryInfo(str);
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = info.Name;
                item.Text = info.Name;
                if (info.Name == "url") continue;
                item.Click += new EventHandler(ToolStripMenuItem_Click);
                contextMenuStrip1.Items.Insert(0, item);
            }


            //newForm = new StartProccess(info.Name);
            //  newForm.LoadUrl();


            // newForm.Show();

            leftform = new leftmenu();
            // leftform.Show();

            this.Hide();
        }


        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = sender.ToString();

            Form newForm1 = CheckMdiFormIsOpen("StartProccess");
            if (newForm1 != null)
            {
                //newForm1.WindowState = FormWindowState.Normal;
                //newForm1.Show();
                newForm1.Close();
                StartProccess from = new StartProccess(sender.ToString());
                from.LoadUrl();
                from.Show();
                

            }
            else
            {
                StartProccess from = new StartProccess(sender.ToString());
                from.LoadUrl();
                from.Show();
                
            }



        }


        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form newForm1 = CheckMdiFormIsOpen("StartProccess");
            //if (newForm1 == null)
            //{
            //    StartProccess from = new StartProccess();
            //    from.Show();
            //}
            //else
            //{
            //    newForm1.WindowState = FormWindowState.Normal;
            //    newForm1.Show();
            //}
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

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Form newForm1 = CheckMdiFormIsOpen("StartProccess");
            //if (newForm1 == null)
            //{
            //    StartProccess from = new StartProccess();
            //    from.Show();
            //}
            //else
            //{
            //    newForm1.WindowState = FormWindowState.Normal;
            //    newForm1.Show();
            //}
        }
    }
}
