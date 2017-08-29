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

        public string Guid { get; set; }
        public string Url { get; set; }
        public override string Text
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public UrlModel urlModel = new UrlModel();

        public UrlPatte()
        {

            InitializeComponent();
        }

        public void SetModel(string guid, string url, string name, string username, string pwd)
        {
            urlModel.guid = guid;
            urlModel.url = url;
            urlModel.name = name;
            urlModel.username = username;
            urlModel.password = pwd;
            Text = name;
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
            CopyUserNamePwd();
            System.Diagnostics.Process.Start(urlModel.url);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(urlModel.url);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            CopyUserNamePwd();
            System.Diagnostics.Process.Start(urlModel.url);
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            //IE 
            CopyUserNamePwd();
            BrowserHelper.OpenIe(urlModel.url);
        }

        private void toolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            //火狐
            CopyUserNamePwd();
            BrowserHelper.OpenFireFox(urlModel.url);
        }

        private void toolStripMenuItem3_Click_1(object sender, EventArgs e)
        {
            //谷歌
            CopyUserNamePwd();
            BrowserHelper.OpenBrowserUrl(urlModel.url);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            CopyUserNamePwd();
            //默认打开
            BrowserHelper.OpenDefaultBrowserUrl(urlModel.url);
            // System.Diagnostics.Process.Start(urlModel.url);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            CopyUserNamePwd();
            BrowserHelper.Open360(urlModel.url);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            CreatUrl create = new CreatUrl();
            create.urlModel = new Cmd.UrlModel();

            create.urlModel.url = urlModel.url;
            create.urlModel.guid = urlModel.guid;
            create.urlModel.name = urlModel.name;
            create.urlModel.password = urlModel.password;
            create.urlModel.username = urlModel.username;
            create.Show();
        }

        private void CopyUserNamePwd()
        {
            string usernamepwd = string.Format("用户名密码：{0}-{1}", urlModel.username, urlModel.password);

            Clipboard.SetDataObject(usernamepwd, true);

        }
    }
}
