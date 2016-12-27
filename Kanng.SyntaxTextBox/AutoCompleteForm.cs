using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;

namespace Kanng.SyntaxTextBox
{
	/// <summary>
	/// Summary description for AutoCompleteForm.
	/// </summary>
	public class AutoCompleteForm : System.Windows.Forms.Form
	{
		private StringCollection mItems = new StringCollection();
        private System.Windows.Forms.ColumnHeader colHeader;
		private System.Windows.Forms.ListView lstCompleteItems;
        public SyntaxTextBox mParent;


		public StringCollection Items 
		{
			get {return mItems;}
		}

		internal int ItemHeight 
		{
			get {return 18;}
		}


        public string SelectedItem 
        {
            get
            {
                if (lstCompleteItems.SelectedItems.Count == 0) return null;
                return (string)lstCompleteItems.SelectedItems[0].Text;
            }
        }

        internal int SelectedIndex 
        {
            get 
            {
                if (lstCompleteItems.SelectedIndices.Count == 0)
                    return -1;
                return lstCompleteItems.SelectedIndices[0];
            }
            set
            {
                lstCompleteItems.Items[value].Selected = true;
                //this.Focus();
                //this.mParent.Focus();
                //lstCompleteItems.Invalidate();
            }
        }


		

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lstCompleteItems = new System.Windows.Forms.ListView();
            this.colHeader = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lstCompleteItems
            // 
            this.lstCompleteItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                               this.colHeader});
            this.lstCompleteItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCompleteItems.FullRowSelect = true;
            this.lstCompleteItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstCompleteItems.HideSelection = false;
            this.lstCompleteItems.LabelWrap = false;
            this.lstCompleteItems.Location = new System.Drawing.Point(0, 0);
            this.lstCompleteItems.MultiSelect = false;
            this.lstCompleteItems.Name = "lstCompleteItems";
            this.lstCompleteItems.Size = new System.Drawing.Size(128, 146);
            this.lstCompleteItems.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstCompleteItems.TabIndex = 1;
            this.lstCompleteItems.View = System.Windows.Forms.View.Details;
            this.lstCompleteItems.DoubleClick += new System.EventHandler(this.lstCompleteItems_DoubleClick);
            this.lstCompleteItems.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstCompleteItems_KeyUp);
            // 
            // colHeader
            // 
            this.colHeader.Width = 148;
            // 
            // AutoCompleteForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(128, 146);
            this.ControlBox = false;
            this.Controls.Add(this.lstCompleteItems);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(128, 176);
            this.MinimizeBox = false;
            this.Name = "AutoCompleteForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "AutoCompleteForm";
            this.TopMost = true;
            this.VisibleChanged += new System.EventHandler(this.AutoCompleteForm_VisibleChanged);
            this.ResumeLayout(false);

        }
		#endregion

        ///////////////////////////////////////////////////////////////////////
        ///
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        public AutoCompleteForm()
        {
            //this.SetTopLevel(false);
            //this.SendToBack();
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }
        

        ///////////////////////////////////////////////////////////////////////
        ///
        ///////////////////////////////////////////////////////////////////////
        internal void UpdateList()
        {
            lstCompleteItems.Items.Clear();
            foreach (string item in mItems)
            {
                lstCompleteItems.Items.Add(item);
            }
        }

        private void AutoCompleteForm_VisibleChanged(object sender, System.EventArgs e)
        {
            // mItems--(Sort)-->items---->mItems ???
            ArrayList items = new ArrayList(mItems);
            items.Sort(new CaseInsensitiveComparer());
            mItems = new StringCollection();
            mItems.AddRange((string[])items.ToArray(typeof(string)));

            // ColumnHeader control ??? No use ???
            colHeader.Width = lstCompleteItems.Width - 20;
        }

        void AcceptText()
        {
            if (mParent != null)
            {
                mParent.AcceptAutoCompleteItem();
                mParent.Focus();
            }
        }

        void ForgiveText()
        {
            if (mParent != null)
            {
                mParent.HideAutoCompleteForm();
                mParent.Focus();
            }
        }


        private void lstCompleteItems_DoubleClick(object sender, System.EventArgs e)
        {
            AcceptText();
        }

        private void lstCompleteItems_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter || e.KeyCode==Keys.Tab)
            {
                AcceptText();
            }
            if (e.KeyCode==Keys.Escape)
            {
                ForgiveText();
            }
        }

    }
}
