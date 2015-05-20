using Kanng.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace kanng.Cmd
{
    public partial class leftmenu : Form
    {
        public leftmenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }



        private void leftmenu_MouseDown(object sender, MouseEventArgs e)
        {

        }

        bool beginMove = false;
        int currentXPosition = 0;
        int currentYPosition = 0;


        private void button1_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Clicks == 2)
            {
                string[] strs = KanngHelper.ReadAllLines();

                string errorFiles = "";

                foreach (string str in strs)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (File.Exists(str))
                        {
                            Process.Start(str);
                        }
                        else
                        {
                            errorFiles += str + "\r\n";
                        }
                    }
                }
                if (errorFiles.Length > 0)
                {
                    MessageBox.Show("没有启动成功的文件:\r\n" + errorFiles);
                }
            }



            beginMove = true;
            currentXPosition = MousePosition.X;
            currentYPosition = MousePosition.Y;
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = this.Left.ToString();
            label2.Text = this.Right.ToString();
            label3.Text = this.Top.ToString();
            label4.Text = this.Bottom.ToString();

            if (beginMove)
            {
                imagemove = 1;
                this.Left += MousePosition.X - currentXPosition;
                this.Top += MousePosition.Y - currentYPosition;
                currentXPosition = MousePosition.X;
                currentYPosition = MousePosition.Y;
            }


        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            beginMove = false;
        }

        private void leftmenu_Load(object sender, EventArgs e)
        {

            Left = 1200;
            Top = 200;


            timer1.Interval = 100;

            timer1.Enabled = false;

            TopMost = true;
            //checkDockTimer.Stop();
        }

        AnchorStyles anchors;
        const int OFFSET = 30;
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_TOPMOST = 8;
                base.CreateParams.Parent = GetDesktopWindow();
                base.CreateParams.ExStyle |= WS_EX_TOPMOST;
                return base.CreateParams;
            }
        }

        int imagemove = 0;

        const int WM_NCHITTEST = 0x84;
        IntPtr HTCLIENT = (IntPtr)0x1;
        IntPtr HTCAPTION = (IntPtr)0x2;
        const int WM_NCLBUTTONDBLCLK = 0x00A3;
        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == WM_NCLBUTTONDBLCLK)
        //    {
        //        return;
        //    }
        //    if (m.Msg == WM_NCHITTEST)
        //    {
        //        base.WndProc(ref m);
        //        if (m.Result == HTCLIENT)
        //        {
        //            m.HWnd = this.Handle;
        //            m.Result = HTCAPTION;
        //        }
        //        return;
        //    }
        //    base.WndProc(ref m);
        //}


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCLBUTTONDBLCLK)
            {
                return;
            }
            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);
                if (m.Result == HTCLIENT)
                {
                    m.HWnd = this.Handle;
                    m.Result = HTCAPTION;
                }
                return;
            }

            const int WM_MOVING = 0x216;
            switch (m.Msg)
            {
                case WM_NCLBUTTONDBLCLK:

                    break;
                case WM_NCHITTEST:

                    base.WndProc(ref m);
                    if (m.Result == HTCLIENT)
                    {
                        m.HWnd = this.Handle;
                        m.Result = HTCAPTION;
                    }
                    break;
                case WM_MOVING: // 窗体移动的消息，控制窗体不会移出屏幕外
                    int left = Marshal.ReadInt32(m.LParam, 0);
                    int top = Marshal.ReadInt32(m.LParam, 4);
                    int right = Marshal.ReadInt32(m.LParam, 8);
                    int bottom = Marshal.ReadInt32(m.LParam, 12);
                    left = Math.Min(Math.Max(0, left),
                        Screen.PrimaryScreen.Bounds.Width - Width);
                    top = Math.Min(Math.Max(0, top),
                        Screen.PrimaryScreen.Bounds.Height - Height);
                    right = Math.Min(Math.Max(Width, right),
                        Screen.PrimaryScreen.Bounds.Width);
                    bottom = Math.Min(Math.Max(Height, bottom),
                        Screen.PrimaryScreen.Bounds.Height);
                    Marshal.WriteInt32(m.LParam, 0, left);
                    Marshal.WriteInt32(m.LParam, 4, top);
                    Marshal.WriteInt32(m.LParam, 8, right);
                    Marshal.WriteInt32(m.LParam, 12, bottom);
                    anchors = AnchorStyles.None;
                    if (left <= OFFSET) anchors |= AnchorStyles.Left;
                    if (top <= OFFSET) anchors |= AnchorStyles.Top;
                    if (bottom >= Screen.PrimaryScreen.Bounds.Height - OFFSET)
                        anchors |= AnchorStyles.Bottom;
                    if (right >= Screen.PrimaryScreen.Bounds.Width - OFFSET)
                        anchors |= AnchorStyles.Right;
                    timer1.Enabled = anchors != AnchorStyles.None;
                    break;
            }
            base.WndProc(ref m);
        }


        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point Point);
        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        private void timer1_Tick(object sender, EventArgs e)
        {
            IntPtr vHandle = WindowFromPoint(Control.MousePosition);
            while (vHandle != IntPtr.Zero && vHandle != Handle)
                vHandle = GetParent(vHandle);
            if (vHandle == Handle) // 如果鼠标停留的窗体是本窗体，还原位置
            {
                if ((anchors & AnchorStyles.Left) == AnchorStyles.Left) Left = 0;
                if ((anchors & AnchorStyles.Top) == AnchorStyles.Top) Top = 0;
                if ((anchors & AnchorStyles.Right) == AnchorStyles.Right)
                    Left = Screen.PrimaryScreen.Bounds.Width - Width;
                if ((anchors & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                    Top = Screen.PrimaryScreen.Bounds.Height - Height;
            }
            else // 隐藏起来
            {
                //if ((anchors & AnchorStyles.Left) == AnchorStyles.Left)
                //    Left = -Width + OFFSET;
                //if ((anchors & AnchorStyles.Top) == AnchorStyles.Top)
                //    Top = -Height + OFFSET;
                if ((anchors & AnchorStyles.Right) == AnchorStyles.Right)
                    Left = Screen.PrimaryScreen.Bounds.Width - OFFSET;
                //if ((anchors & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                //    Top = Screen.PrimaryScreen.Bounds.Height - OFFSET;
            }

        }

        private void 关闭智能启动功能ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        StartProccess newForm = null;
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

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }
}
