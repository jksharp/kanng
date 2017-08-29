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
       
        public UrlModel urlModel
        {
            get;
            set;
        }

        public CreatUrl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "")
            {
                return;
            }

            string guid = Guid.NewGuid().ToString().Replace("-", "");

            if (urlModel != null)
            {
                guid = urlModel.guid;
                bool rlb = UrlXmlIO.UpdateNode(textBox2.Text, textBox1.Text, guid,txt_UserName.Text,txt_Pwd.Text);
                if (rlb) MessageBox.Show("更新成功!");

                StartProccess startFrom = (StartProccess)CheckMdiFormIsOpen("StartProccess");
                UrlModel model = new UrlModel();
                model.guid = guid;
                model.name = textBox2.Text;
                model.url = textBox1.Text;
                model.username = txt_UserName.Text.Trim();
                model.password = txt_Pwd.Text.Trim();

                startFrom.updateControl(model);
            }
            else
            {
                bool rlb = UrlXmlIO.Create(textBox2.Text, textBox1.Text, guid, txt_UserName.Text, txt_Pwd.Text);
                if (rlb) MessageBox.Show("添加成功!");

                StartProccess startFrom = (StartProccess)CheckMdiFormIsOpen("StartProccess");
                UrlModel model = new UrlModel();
                model.guid = guid;
                model.name = textBox2.Text;
                model.url = textBox1.Text;
                model.username = txt_UserName.Text.Trim();
                model.password = txt_Pwd.Text.Trim();
                startFrom.addControl(model);
            }         
          
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

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri( textBox1.Text);
        }

        private void CreatUrl_Load(object sender, EventArgs e)
        {

            if (urlModel != null)
            {
                textBox1.Text = urlModel.url;
                textBox2.Text = urlModel.name;
                txt_UserName.Text = urlModel.username;
                txt_Pwd.Text = urlModel.password;
                try
                {
                    if (!textBox1.Text.StartsWith("http")) textBox1.Text = "http://" + textBox1.Text;
                    webBrowser1.Url = new Uri(textBox1.Text);
                }
                catch { };
            }

            webBrowser1.ScriptErrorsSuppressed = false;
        }
    }
}
