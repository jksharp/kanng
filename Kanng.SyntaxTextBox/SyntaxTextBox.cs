using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using System.Xml.Serialization;


namespace Kanng.SyntaxTextBox
{
	/// <summary>
	/// A textbox the does syntax .
	/// </summary>
    [XmlInclude(typeof(DescriptorType))]
    [XmlInclude(typeof(DescriptorRecognition))]
    [XmlInclude(typeof(SeperatorCollection))]
    [XmlInclude(typeof(Descriptor))]
    [XmlInclude(typeof(DescriptorCollection))]
    public class SyntaxTextBox : System.Windows.Forms.RichTextBox 
	{
		#region Members

		//Members exposed via properties
		private SeperatorCollection mSeperators = new SeperatorCollection();  
		private DescriptorCollection mDescriptors = new DescriptorCollection();
		private bool mCaseSesitive = false;
		private bool mFilterAutoComplete = false;

		//Internal use members
		private bool mAutoCompleteShown = false;
		private bool mParsing = false;

		//Undo/Redo members
		private ArrayList mUndoList = new ArrayList();
		private Stack mRedoStack = new Stack();
		private bool mIsUndo = false;
		private UndoRedoInfo mLastInfo = new UndoRedoInfo("", new Win32.POINT(), 0);
		private int mMaxUndoRedoSteps = 50;

        //AutoCompleteForm
        private AutoCompleteForm mAutoForm = new AutoCompleteForm();

		#endregion

		#region Properties
		/// <summary>
		/// Determines if token recognition is case sensitive.
		/// </summary>
		[Category("Behavior")]
		public bool CaseSensitive 
		{ 
			get {return mCaseSesitive; }
			set {mCaseSesitive = value;}
		}


		/// <summary>
		/// Sets whether or not to remove items from the Autocomplete window as the user types...
		/// </summary>
		[Category("Behavior")]
		public bool FilterAutoComplete 
		{
			get {return mFilterAutoComplete;}
			set {mFilterAutoComplete = value;}
		}

		/// <summary>
		/// Set the maximum amount of Undo/Redo steps.
		/// </summary>
		[Category("Behavior")]
		public int MaxUndoRedoSteps 
		{
			get {return mMaxUndoRedoSteps;}
			set	{mMaxUndoRedoSteps = value;}
		}

		/// <summary>
		/// A collection of charecters. a token is every string between two seperators.
		/// </summary>
		/// 
		public SeperatorCollection Seperators 
		{
			get {return mSeperators;}
		}
		
		/// <summary>
		/// The collection of  descriptors.
		/// </summary>
		public DescriptorCollection Descriptors 
		{
			get {return mDescriptors;}
		}

		#endregion

        #region ConfigFile
        ///////////////////////////////////////////////////////////////////////////////
        /// Xml config file
        ///////////////////////////////////////////////////////////////////////////////
        private string mConfigFile = "csharp.xml";
        public string ConfigFile
        {
            set 
            {
                LoadConfig(value);
                mConfigFile = value;
                this.OnTextChanged(null);
            }
            get
            {
                return mConfigFile;
            }
        }

        public void LoadConfig(string xmlFile)
        {
            try 
            {
                mDescriptors = (DescriptorCollection)Common.LoadXml(xmlFile, mDescriptors.GetType());
            }
            catch (Exception ex)
            {
                mDescriptors.LoadDefaultData();
                string str = string.Format("{0}. \r\nProgram will load the default config.", ex.Message);
                //MessageBox.Show(this, str, "Error");
            }
        }

        public void SaveConfig(string xmlFile)
        {
            try
            {
                Common.SaveXml(xmlFile, mDescriptors);
            }
            catch
            {
                MessageBox.Show(this, "Save error", "Error");
            }
        }
        #endregion

        #region <Kernal>
        public SyntaxTextBox()
        {
            this.AcceptsTab = true;
            this.WordWrap = false;
            this.ScrollBars = RichTextBoxScrollBars.Both;
            this.FilterAutoComplete = true;

            //SaveConfig("cs.xml");
            //LoadConfig("cs.xml");
            LoadConfig("csharp.xml");
        }



		/// <summary>
		/// The on text changed overrided. Here we parse the text into RTF for the .
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged(EventArgs e)
		{
			if (mParsing) return;
			mParsing = true;
			Win32.LockWindowUpdate(Handle);
			base.OnTextChanged(e);

			if (!mIsUndo)
			{
				mRedoStack.Clear();
				mUndoList.Insert(0, mLastInfo);
				this.LimitUndo();
				mLastInfo = new UndoRedoInfo(Text, GetScrollPos(), SelectionStart);
			}
			
			//Save scroll bar an cursor position, changeing the RTF moves the cursor and scrollbars to top positin
			Win32.POINT scrollPos = GetScrollPos();
			int cursorLoc = SelectionStart;

			//Created with an estimate of how big the stringbuilder has to be...
			StringBuilder sb = new StringBuilder((int)(Text.Length * 1.5 + 150));
	
            //Kevin.C modify 2005-12
            //Adding RTF header
            //sb.Append(@"{\rtf1\ansi\ansicpg936\deff0\deflang1033\deflangfe2052{\fonttbl{");
            sb.Append(@"{\rtf1\fbidis\ansi\ansicpg1255\deff0\deflang1037\deftab380{\fonttbl{");
			
			//Font table creation
			int fontCounter = 0;
			Hashtable fonts = new Hashtable();
			AddFontToTable(sb, Font, ref fontCounter, fonts);
			foreach (Descriptor desc in mDescriptors)
			{
				if ((desc.Font !=  null) && !fonts.ContainsKey(desc.Font.Name))
				{
					AddFontToTable(sb, desc.Font, ref fontCounter, fonts);
				}
			}
			sb.Append("}\n");

			//ColorTable
            int colorCounter = 1;
            sb.Append(@"{\colortbl ;");
			Hashtable colors = new Hashtable();
			AddColorToTable(sb, ForeColor, ref colorCounter, colors);
			AddColorToTable(sb, BackColor, ref colorCounter, colors);
			foreach (Descriptor desc in mDescriptors)
			{
				if (!colors.ContainsKey(desc.Color))
				{
					AddColorToTable(sb, desc.Color, ref colorCounter, colors);
				}
			}		

			//Parsing text
			sb.Append("}\n").Append(@"\viewkind4\uc1\pard\ltrpar");
			SetDefaultSettings(sb, colors, fonts);

            //Seperators
            char[] sperators = mSeperators.GetAsCharArray();

			//Replacing "\" to "\\" for RTF...
			string[] lines = Text.Replace("\\","\\\\").Replace("{", "\\{").Replace("}", "\\}").Split('\n');
			for (int lineCounter = 0 ; lineCounter < lines.Length; lineCounter++)
			{
				if (lineCounter != 0)
				{
					AddNewLine(sb);
				}
				string line = lines[lineCounter];
				string[] tokens = mCaseSesitive ? line.Split(sperators) : line.ToUpper().Split(sperators);
				if (tokens.Length == 0)
				{
					sb.Append(line);
					AddNewLine(sb);
					continue;
				}

				int tokenCounter = 0;
				for (int i = 0; i < line.Length ;)
				{
					char curChar = line[i];
					if (mSeperators.Contains(curChar))
					{
						sb.Append(curChar);
						i++;
					}
					else
					{
						string curToken = tokens[tokenCounter++];
						bool bAddToken = true;
						foreach	(Descriptor desc in mDescriptors)
						{
							string	compareStr = mCaseSesitive ? desc.Token : desc.Token.ToUpper();
							bool match = false;

							//Check if the  descriptor matches the current toker according to the DescriptoRecognision property.
							switch	(desc.DescriptorRecognition)
							{
								case DescriptorRecognition.WholeWord:
									if (curToken == compareStr)
									{
											match = true;
									}
									break;
								case DescriptorRecognition.StartsWith:
									if (curToken.StartsWith(compareStr))
									{
										match = true;
									}
									break;
								case DescriptorRecognition.Contains:
									if (curToken.IndexOf(compareStr) != -1)
									{
										match = true;
									}
									break;
							}
							if (!match)
							{
								//If this token doesn't match chech the next one.
								continue;
							}

							//printing this token will be handled by the inner code, don't apply default settings...
							bAddToken = false;
	
							//Set colors to current descriptor settings.
							SetDescriptorSettings(sb, desc, colors, fonts);

							//Print text affected by this descriptor.
							switch (desc.DescriptorType)
							{
								case DescriptorType.Word:
									sb.Append(line.Substring(i, curToken.Length));
									SetDefaultSettings(sb, colors, fonts);
									i += curToken.Length;
									break;
								case DescriptorType.ToEOL:
									sb.Append(line.Remove(0, i));
									i = line.Length;
									SetDefaultSettings(sb, colors, fonts);
									break;
								case DescriptorType.ToCloseToken:
									while((line.IndexOf(desc.CloseToken, i) == -1) && (lineCounter < lines.Length))
									{
										sb.Append(line.Remove(0, i));
										lineCounter++;
										if (lineCounter < lines.Length)
										{
											AddNewLine(sb);
											line = lines[lineCounter];
											i = 0;
										}
										else
										{
											i = line.Length;
										}
									}
									if (line.IndexOf(desc.CloseToken, i) != -1)
									{
										sb.Append(line.Substring(i, line.IndexOf(desc.CloseToken, i) + desc.CloseToken.Length - i) );
										line = line.Remove(0, line.IndexOf(desc.CloseToken, i) + desc.CloseToken.Length);
										tokenCounter = 0;
										tokens = mCaseSesitive ? line.Split(sperators) : line.ToUpper().Split(sperators);
										SetDefaultSettings(sb, colors, fonts);
										i = 0;
									}
									break;
							}
							break;
						}
						if (bAddToken)
						{
							//Print text with default settings...
							sb.Append(line.Substring(i, curToken.Length));
							i+=	curToken.Length;
						}
					}
				}
			}
	
			//System.Diagnostics.Debug.WriteLine(sb.ToString());
			Rtf = sb.ToString();

			//Restore cursor and scrollbars location.
			SelectionStart = cursorLoc;
			mParsing = false;
			SetScrollPos(scrollPos);
			Win32.LockWindowUpdate((IntPtr)0);
			Invalidate();
			

            //Adjust AutoCompleteForm items, size, location, selectItem
			if (mAutoCompleteShown)
			{
				if (mFilterAutoComplete)
				{
					SetAutoCompleteItems();
					SetAutoCompleteSize();
					SetAutoCompleteLocation(true); //...
				}
				SetBestSelectedAutoCompleteItem();
			}
		}
        #endregion

        #region Event

		protected override void OnVScroll(EventArgs e)
		{
			if (mParsing) return;
			base.OnVScroll (e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//HideAutoCompleteForm();
			base.OnMouseDown (e);
		}

		/// <summary>
		/// Taking care of Keyboard events
		/// </summary>
		/// <param name="m"></param>
		/// <remarks>
		/// Since even when overriding the OnKeyDown methoed and not calling the base function 
		/// you don't have full control of the input, I've decided to catch windows messages to handle them.
		/// </remarks>
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case Win32.WM_PAINT:
				{
					//Don't draw the control while parsing to avoid flicker.
					if (mParsing)
					{
						return;
					}
					break;
				}
				case Win32.WM_KEYDOWN:
				{
					if (mAutoCompleteShown)
					{
						switch ((Keys)(int)m.WParam)
						{
                            case Keys.Down:
							{
								if (mAutoForm.Items.Count != 0)
									mAutoForm.SelectedIndex = (mAutoForm.SelectedIndex + 1) % mAutoForm.Items.Count;
								return;
							}
							case Keys.Up:
							{
								if (mAutoForm.Items.Count != 0)
                                    mAutoForm.SelectedIndex = (mAutoForm.SelectedIndex + mAutoForm.Items.Count - 1) % mAutoForm.Items.Count;
								return;
							}
                            case Keys.Home:
                            {
                                mAutoForm.SelectedIndex = (mAutoForm.Items.Count==0) ? -1 : 0;
                                return;
                            }
                            case Keys.End:
                            {
                                mAutoForm.SelectedIndex = mAutoForm.Items.Count - 1;
                                return;
                            }
                            case Keys.PageDown:
                            {
                                if (mAutoForm.Items.Count != 0)
                                    mAutoForm.SelectedIndex = (mAutoForm.SelectedIndex + 10) % mAutoForm.Items.Count;
                                return;
                            }
                            case Keys.PageUp:
                            {
                                if (mAutoForm.Items.Count != 0)
                                    mAutoForm.SelectedIndex = (mAutoForm.SelectedIndex + mAutoForm.Items.Count - 1) % mAutoForm.Items.Count;
                                return;
                            }
                            case Keys.Tab:
							case Keys.Enter:
							{
								AcceptAutoCompleteItem();
								return;
							}
							case Keys.Escape:
							{
								HideAutoCompleteForm();
								return;
							}
						}
                        this.Focus();
					}
					else
					{
                        // ctrl + shift + space
						if (((Keys)(int)m.WParam == Keys.Space) && 
							((Win32.GetKeyState(Win32.VK_CONTROL) & Win32.KS_KEYDOWN) != 0))
						{
							CompleteWord();
						} 
						else if (((Keys)(int)m.WParam == Keys.Z) && 
							((Win32.GetKeyState(Win32.VK_CONTROL) & Win32.KS_KEYDOWN) != 0))
						{
							Undo();
							return;
						}
						else if (((Keys)(int)m.WParam == Keys.Y) && 
							((Win32.GetKeyState(Win32.VK_CONTROL) & Win32.KS_KEYDOWN) != 0))
						{
							Redo();
							return;
						}
                        // Kevin.C added, 2005-12
                        else if (((Keys)(int)m.WParam == Keys.A) && 
                            ((Win32.GetKeyState(Win32.VK_CONTROL) & Win32.KS_KEYDOWN) != 0))
                        {
                            this.SelectAll();
                            return;
                        }
                    }
					break;
				}
				case Win32.WM_CHAR:
				{
					switch ((Keys)(int)m.WParam)
					{
						case Keys.Space:
							if ((Win32.GetKeyState(Win32.VK_CONTROL) & Win32.KS_KEYDOWN )!= 0)
							{
								return;
							}
							break;
						case Keys.Enter:
							if (mAutoCompleteShown) return;
							break;
					}
				}
				break;

			}
			base.WndProc(ref m);
		}


		/// <summary>
		/// Hides the AutoComplete form when losing focus on textbox.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus (e);
		}


		#endregion

		#region Undo/Redo Code
		public new bool CanUndo 
		{
			get {return mUndoList.Count > 0;}
		}
		public new bool CanRedo
		{
			get {return mRedoStack.Count > 0;}
		}

		private void LimitUndo()
		{
			while (mUndoList.Count > mMaxUndoRedoSteps)
			{
				mUndoList.RemoveAt(mMaxUndoRedoSteps);
			}
		}

		public new void Undo()
		{
			if (!CanUndo)
				return;
			mIsUndo = true;
			mRedoStack.Push(new UndoRedoInfo(Text, GetScrollPos(), SelectionStart));
			UndoRedoInfo info = (UndoRedoInfo)mUndoList[0];
			mUndoList.RemoveAt(0);
			Text = info.Text;
			SelectionStart = info.CursorLocation;
			SetScrollPos(info.ScrollPos);
			mLastInfo = info;
			mIsUndo = false;
		}

        public new void Redo()
		{
			if (!CanRedo)
				return;
			mIsUndo = true;
			mUndoList.Insert(0,new UndoRedoInfo(Text, GetScrollPos(), SelectionStart));
			LimitUndo();
			UndoRedoInfo info = (UndoRedoInfo)mRedoStack.Pop();
			Text = info.Text;
			SelectionStart = info.CursorLocation;
			SetScrollPos(info.ScrollPos);
			mIsUndo = false;
		}

		private class UndoRedoInfo
		{
			public UndoRedoInfo(string text, Win32.POINT scrollPos, int cursorLoc)
			{
				Text = text;
				ScrollPos = scrollPos;
				CursorLocation = cursorLoc;
			}
			public readonly Win32.POINT ScrollPos;
			public readonly int CursorLocation;
			public readonly string Text;
		}
		#endregion

		#region AutoComplete functions

		/// <summary>
		/// Entry point to autocomplete mechanism.
		/// Tries to complete the current word. if it fails it shows the AutoComplete form.
        /// Kevin.C modified 2005-12
        /// </summary>
		public void CompleteWord()
		{
			int curTokenStartIndex = Text.LastIndexOfAny(mSeperators.GetAsCharArray(), Math.Max(SelectionStart-1, 0)) + 1;
			int curTokenEndIndex = Text.IndexOfAny(mSeperators.GetAsCharArray(), SelectionStart);
			if (curTokenEndIndex == -1) curTokenEndIndex = Text.Length;
            string curTokenString = Text.Substring(curTokenStartIndex, Math.Max(curTokenEndIndex-curTokenStartIndex, 0)).ToUpper();

            string token = null;
            foreach (Descriptor desc in mDescriptors)
			{
				if (desc.UseForAutoComplete && desc.Token.ToUpper().StartsWith(curTokenString))
				{
					if (token == null)
						token = desc.Token;
					else
					{
						token = null;
						break;
					}
				}
			}
			if (token == null)
			{
				ShowAutoComplete();
			}
			else
			{
				SelectionStart = curTokenStartIndex;
				SelectionLength = curTokenEndIndex - curTokenStartIndex;
				SelectedText = token;
				SelectionStart = SelectionStart + SelectionLength;
				SelectionLength = 0;
			}
		}

		/// <summary>
		/// replace the current word of the cursor with the one from the AutoComplete form and closes it.
		/// </summary>
		/// <returns>If the operation was succesful</returns>
		public bool AcceptAutoCompleteItem()
		{
			if (mAutoForm.SelectedItem == null)
			{
				return false;
			}
			
			int curTokenStartIndex = Text.LastIndexOfAny(mSeperators.GetAsCharArray(), Math.Max(SelectionStart-1, 0)) + 1;
			int curTokenEndIndex= Text.IndexOfAny(mSeperators.GetAsCharArray(), SelectionStart);
			if (curTokenEndIndex == -1) curTokenEndIndex = Text.Length;

            SelectionStart = Math.Max(curTokenStartIndex, 0);
			SelectionLength = Math.Max(0,curTokenEndIndex - curTokenStartIndex);
			SelectedText = mAutoForm.SelectedItem;
			SelectionStart = SelectionStart + SelectionLength;
			SelectionLength = 0;
			
			HideAutoCompleteForm();
			return true;
		}



		/// <summary>
		/// Finds the and sets the best matching token as the selected item in the AutoCompleteForm.
		/// </summary>
		private void SetBestSelectedAutoCompleteItem()
		{
			int curTokenStartIndex = Text.LastIndexOfAny(mSeperators.GetAsCharArray(), Math.Max(SelectionStart-1, 0)) + 1;
			int curTokenEndIndex= Text.IndexOfAny(mSeperators.GetAsCharArray(), SelectionStart);
			if (curTokenEndIndex == -1) curTokenEndIndex = Text.Length;
			string curTokenString = Text.Substring(curTokenStartIndex, Math.Max(curTokenEndIndex - curTokenStartIndex,0)).ToUpper();
			
			if ((mAutoForm.SelectedItem != null) && mAutoForm.SelectedItem.ToUpper().StartsWith(curTokenString))
			{
				return;
			}

			int matchingChars = -1;
			string bestMatchingToken = null;

			foreach (string item in mAutoForm.Items)
			{
				bool isWholeItemMatching = true;
				for (int i = 0 ; i < Math.Min(item.Length, curTokenString.Length); i++)
				{
					if (char.ToUpper(item[i]) != char.ToUpper(curTokenString[i]))
					{
						isWholeItemMatching = false;
						if (i-1 > matchingChars)
						{
							matchingChars = i;
							bestMatchingToken = item;
							break;
						}
					}
				}
				if (isWholeItemMatching && (Math.Min(item.Length, curTokenString.Length) > matchingChars))
				{
					matchingChars = Math.Min(item.Length, curTokenString.Length);
					bestMatchingToken = item;
				}
			}
			
			if (bestMatchingToken != null)
			{
				mAutoForm.SelectedIndex = mAutoForm.Items.IndexOf(bestMatchingToken);
			}
		}

		/// <summary>
		/// Sets the items for the AutoComplete form.
		/// Kevin.C modified 2005-12
		/// </summary>
		private void SetAutoCompleteItems()
		{
			mAutoForm.Items.Clear();
			string filterString = "";
			if (mFilterAutoComplete)
			{
				int filterTokenStartIndex = Text.LastIndexOfAny(mSeperators.GetAsCharArray(), Math.Max(SelectionStart-1, 0)) + 1;
				int filterTokenEndIndex = Text.IndexOfAny(mSeperators.GetAsCharArray(), SelectionStart);
				if (filterTokenEndIndex == -1) filterTokenEndIndex = Text.Length;
				filterString = Text.Substring(filterTokenStartIndex, Math.Max(filterTokenEndIndex-filterTokenStartIndex, 0)).ToUpper();
			}
		
			foreach (Descriptor desc in mDescriptors)
			{
				if (desc.UseForAutoComplete && desc.Token.ToUpper().StartsWith(filterString))
				{
					mAutoForm.Items.Add(desc.Token);
				}
			}
			mAutoForm.UpdateList();
		}
		
		/// <summary>
		/// Sets the size. the size is limited by the MaxSize property in the form itself.
		/// </summary>
		private void SetAutoCompleteSize()
		{
			mAutoForm.Height = Math.Min(
				Math.Max(mAutoForm.Items.Count, 1) * mAutoForm.ItemHeight + 4, 
				mAutoForm.MaximumSize.Height);
		}

		/// <summary>
		/// closes the AutoCompleteForm.
		/// </summary>
		public void HideAutoCompleteForm()
		{
			mAutoForm.Visible = false;
			mAutoCompleteShown = false;
		}
		

		/// <summary>
		/// Sets the location of the AutoComplete form, maiking sure it's on the screen where the cursor is.
		/// </summary>
		/// <param name="moveHorizontly">determines wheather or not to move the form horizontly.</param>
		private void SetAutoCompleteLocation(bool moveHorizontly)
		{
			Point cursorLocation = GetPositionFromCharIndex(SelectionStart);
			Screen screen = Screen.FromPoint(cursorLocation);
			Point optimalLocation = new Point(PointToScreen(cursorLocation).X-15, (int)(PointToScreen(cursorLocation).Y + Font.Size*2 + 2));
			Rectangle desiredPlace = new Rectangle(optimalLocation , mAutoForm.Size);
			desiredPlace.Width = 152;
			if (desiredPlace.Left < screen.Bounds.Left) 
			{
				desiredPlace.X = screen.Bounds.Left;
			}
			if (desiredPlace.Right > screen.Bounds.Right)
			{
				desiredPlace.X -= (desiredPlace.Right - screen.Bounds.Right);
			}
			if (desiredPlace.Bottom > screen.Bounds.Bottom)
			{
				desiredPlace.Y = cursorLocation.Y - 2 - desiredPlace.Height;
			}
			if (!moveHorizontly)
			{
				desiredPlace.X = mAutoForm.Left;
			}

			mAutoForm.Bounds = desiredPlace;
		}

		/// <summary>
		/// Shows the Autocomplete form.
		/// </summary>
		public void ShowAutoComplete()
		{
			SetAutoCompleteItems();
			SetAutoCompleteSize();
			SetAutoCompleteLocation(true);
			
            // add by Kevin.C 2005-12
            mAutoForm.mParent = this;
			mAutoForm.Visible = true;

			SetBestSelectedAutoCompleteItem();
			mAutoCompleteShown = true;
			Focus();
		}

		#endregion 

		#region Rtf building helper functions

		/// <summary>
		/// Set color and font to default control settings.
		/// </summary>
		/// <param name="sb">the string builder building the RTF</param>
		/// <param name="colors">colors hashtable</param>
		/// <param name="fonts">fonts hashtable</param>
		private void SetDefaultSettings(StringBuilder sb, Hashtable colors, Hashtable fonts)
		{
			SetColor(sb, ForeColor, colors);
			SetFont(sb, Font, fonts);
			SetFontSize(sb, (int)Font.Size);
			EndTags(sb);
		}

		/// <summary>
		/// Set Color and font to a  descriptor settings.
		/// </summary>
		/// <param name="sb">the string builder building the RTF</param>
		/// <param name="hd">the Descriptor with the font and color settings to apply.</param>
		/// <param name="colors">colors hashtable</param>
		/// <param name="fonts">fonts hashtable</param>
		private void SetDescriptorSettings(StringBuilder sb, Descriptor hd, Hashtable colors, Hashtable fonts)
		{
			SetColor(sb, hd.Color, colors);
			if (hd.Font != null)
			{
				SetFont(sb, hd.Font, fonts);
				SetFontSize(sb, (int)hd.Font.Size);
			}
			EndTags(sb);

		}
		/// <summary>
		/// Sets the color to the specified color
		/// </summary>
		private void SetColor(StringBuilder sb, Color color, Hashtable colors)
		{
			sb.Append(@"\cf").Append(colors[color]);
		}
		/// <summary>
		/// Sets the backgroung color to the specified color.
		/// </summary>
		private void SetBackColor(StringBuilder sb, Color color, Hashtable colors)
		{
			sb.Append(@"\cb").Append(colors[color]);
		}
		/// <summary>
		/// Sets the font to the specified font.
		/// </summary>
		private void SetFont(StringBuilder sb, Font font, Hashtable fonts)
		{
			if (font == null) return;
			sb.Append(@"\f").Append(fonts[font.Name]);
		}
		/// <summary>
		/// Sets the font size to the specified font size.
		/// </summary>
		private void SetFontSize(StringBuilder sb, int size)
		{
			sb.Append(@"\fs").Append(size*2);
		}
		/// <summary>
		/// Adds a newLine mark to the RTF.
		/// </summary>
		private void AddNewLine(StringBuilder sb)
		{
			sb.Append("\\par\n");
		}

		/// <summary>
		/// Ends a RTF tags section.
		/// </summary>
		private void EndTags(StringBuilder sb)
		{
			sb.Append(' ');
		}



		/// <summary>
		/// Adds a font to the RTF's font table and to the fonts hashtable.
		/// </summary>
		/// <param name="sb">The RTF's string builder</param>
		/// <param name="font">the Font to add</param>
		/// <param name="counter">a counter, containing the amount of fonts in the table</param>
		/// <param name="fonts">an hashtable. the key is the font's name. the value is it's index in the table</param>
		private void AddFontToTable(StringBuilder sb, Font font, ref int counter, Hashtable fonts)
		{
            // Kevin.C modify 2005-12
            // Support ascii char only
			// sb.Append(@"\f").Append(counter).Append(@"\fnil\fcharset0").Append(font.Name).Append(";}");
            // Support unicode char !!!
            sb.Append(@"\f").Append(counter).Append(@"\fnil\fprq2\fcharset134").Append(font.Name).Append(";}");
            fonts.Add(font.Name, counter++);
		}


		/// <summary>
		/// Adds a color to the RTF's color table and to the colors hashtable.
		/// </summary>
		/// <param name="sb">The RTF's string builder</param>
		/// <param name="color">the color to add</param>
		/// <param name="counter">a counter, containing the amount of colors in the table</param>
		/// <param name="colors">an hashtable. the key is the color. the value is it's index in the table</param>
		private void AddColorToTable(StringBuilder sb, Color color, ref int counter, Hashtable colors)
		{
	
			sb.Append(@"\red").Append(color.R).Append(@"\green").Append(color.G).Append(@"\blue").Append(color.B).Append(";");
			colors.Add(color, counter++);
		}

		#endregion

		#region Scrollbar positions functions
		/// <summary>
		/// Sends a win32 message to get the scrollbars' position.
		/// </summary>
		/// <returns>a POINT structore containing horizontal and vertical scrollbar position.</returns>
		private unsafe Win32.POINT GetScrollPos()
		{
			Win32.POINT res = new Win32.POINT();
			IntPtr ptr = new IntPtr(&res);
			Win32.SendMessage(Handle, Win32.EM_GETSCROLLPOS, 0, ptr);
			return res;

		}

		/// <summary>
		/// Sends a win32 message to set scrollbars position.
		/// </summary>
		/// <param name="point">a POINT conatining H/Vscrollbar scrollpos.</param>
		private unsafe void SetScrollPos(Win32.POINT point)
		{
			IntPtr ptr = new IntPtr(&point);
			Win32.SendMessage(Handle, Win32.EM_SETSCROLLPOS, 0, ptr);
		}
		#endregion
	}

}