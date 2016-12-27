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
    public partial class CreatUrl : Form
    {
        public CreatUrl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");

            bool rlb = UrlXmlIO.Create(textBox1.Text, guid);
            if (rlb) MessageBox.Show("添加成功!");

            StartProccess startFrom = (StartProccess)CheckMdiFormIsOpen("StartProccess");
            UrlModel model = new UrlModel();
            model.guid = guid;
            model.name = textBox1.Text;
            model.url = textBox1.Text;
            startFrom.addControl(model);
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
