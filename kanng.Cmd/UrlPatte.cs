using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace kanng.Cmd
{
    public partial class UrlPatte : UserControl
    {

        public UrlModel urlModel = new UrlModel();

        public UrlPatte()
        {

            InitializeComponent();
        }

        public void SetModel(string guid, string url, string name)
        {

            urlModel.guid = guid;
            urlModel.url = url;
            urlModel.name = name;
        }

        private void UrlPatte_Load(object sender, EventArgs e)
        {
            label1.Text = urlModel.name;
        }

        private void UrlPatte_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2)
            {
                System.Diagnostics.Process.Start("http://" + urlModel.url);
            }


        }


        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(urlModel.url);
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2)
            {
                System.Diagnostics.Process.Start("http://" + urlModel.url);

            }

        }


        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isSuccess = UrlXmlIO.DeleteNode(urlModel.guid);

            MessageBox.Show("XML节点删除成功:" + isSuccess.ToString());
            this.Hide();
            this.Dispose();
        }


    }
}
