using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kanng.Common;

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
            toolTip1.SetToolTip(this, urlModel.url);
         
        }

        private void UrlPatte_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2)
            {
                System.Diagnostics.Process.Start(urlModel.url);
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
                System.Diagnostics.Process.Start(urlModel.url);

            }

        }


        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isSuccess = UrlXmlIO.DeleteNode(urlModel.guid);

            MessageBox.Show("XML节点删除成功:" + isSuccess.ToString());
            this.Hide();
            this.Dispose();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(urlModel.url);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(urlModel.url);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(urlModel.url);
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            //IE 
            BrowserHelper.OpenIe(urlModel.url);
        }

        private void toolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            //火狐
            BrowserHelper.OpenFireFox(urlModel.url);
        }

        private void toolStripMenuItem3_Click_1(object sender, EventArgs e)
        {
            //谷歌

            BrowserHelper.OpenBrowserUrl(urlModel.url);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            //默认打开
            BrowserHelper.OpenDefaultBrowserUrl(urlModel.url);
           // System.Diagnostics.Process.Start(urlModel.url);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            BrowserHelper.Open360(urlModel.url);
        }
    }
}
