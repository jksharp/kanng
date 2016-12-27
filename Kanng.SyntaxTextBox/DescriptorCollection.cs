using System;
using System.Collections;
using System.Xml.Serialization;
using System.Drawing;
using System.Windows.Forms;

namespace Kanng.SyntaxTextBox
{
	/// <summary>
	/// Summary description for SeperatorCollection.
	/// </summary>
    [Serializable()]
    public class DescriptorCollection : ILoadDefaultData, IXmlSerializable
	{
		private ArrayList mInnerList = new ArrayList();

        public DescriptorCollection()
		{
            //LoadDefaultData();
		}

		public void AddRange(ICollection c)
		{
			mInnerList.AddRange(c);
		}


		#region IList Members
        [XmlIgnoreAttribute]
		public bool IsReadOnly
		{
			get {return mInnerList.IsReadOnly;}
		}

        [XmlIgnoreAttribute]
        public Descriptor this[int index]
		{
			get {return (Descriptor)mInnerList[index];}
			set	{mInnerList[index] = value;}
		}

        [XmlIgnoreAttribute]
        public bool IsFixedSize
        {
            get {return mInnerList.IsFixedSize;}
        }

		public void RemoveAt(int index)
		{
			mInnerList.RemoveAt(index);
		}

		public void Insert(int index, Descriptor value)
		{
			mInnerList.Insert(index, value);
		}

		public void Remove(Descriptor value)
		{
			mInnerList.Remove(value);
		}

		public bool Contains(Descriptor value)
		{
			return mInnerList.Contains(value);
		}

		public void Clear()
		{
			mInnerList.Clear();
		}

		public int IndexOf(Descriptor value)
		{
			return mInnerList.IndexOf(value);
		}

		public int Add(Descriptor value)
		{
			return mInnerList.Add(value);
		}

		#endregion

        #region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return mInnerList.GetEnumerator();
		}

		#endregion

        #region ILoadDefaultData 成员
        public void LoadDefaultData()
        {
            Clear();
            Add(new Descriptor("#", Color.DarkGreen, null, DescriptorType.ToEOL, DescriptorRecognition.StartsWith, false));
            Add(new Descriptor("/*", "*/",  Color.DarkGreen, null, DescriptorType.ToCloseToken, DescriptorRecognition.StartsWith, false));
            Add(new Descriptor("'", "'",    Color.Red,       null, DescriptorType.ToCloseToken, DescriptorRecognition.StartsWith, false));
            Add(new Descriptor("//",        Color.DarkGreen, null, DescriptorType.ToEOL,        DescriptorRecognition.StartsWith, false));


            Add(new Descriptor("#region",   Color.Blue,       null, DescriptorType.Word,        DescriptorRecognition.WholeWord,  false));
            Add(new Descriptor("#endregion",Color.Blue,       null, DescriptorType.Word,        DescriptorRecognition.WholeWord,  false));

            Add(new Descriptor("public",    Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("protected", Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("private",   Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("readonly",  Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("const",     Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("static",    Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));

            Add(new Descriptor("get",       Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("set",       Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));


            Add(new Descriptor("namespace", Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("class",     Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("struct",    Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("interface", Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("enum",      Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));


            Add(new Descriptor("as",        Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("is",        Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("typeof",    Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("return",    Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("using",     Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("unsafe",    Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("yield",     Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("lock",      Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("try",       Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("catch",     Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("finnaly",   Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("throw",     Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));

            Add(new Descriptor("if",        Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("else",      Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("while",     Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("goto",      Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("for",       Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("foreach",   Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("in",        Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("break",     Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("return",    Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));

            Add(new Descriptor("void",      Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("string",    Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("char",      Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("byte",      Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("int",       Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("short",     Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("long",      Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("float",     Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("double",    Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("decimal",   Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("bool",      Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));

            Add(new Descriptor("null",      Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("true",      Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("false",     Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));

            Add(new Descriptor("new",       Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
            Add(new Descriptor("this",      Color.Blue,      null, DescriptorType.Word,         DescriptorRecognition.WholeWord,  true));
        }
        #endregion

        #region IXmlSerializable 成员
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (Descriptor desc in mInnerList)
            {
                writer.WriteStartElement("Descriptor");
                desc.WriteXml(writer);
                writer.WriteEndElement();
            }
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            try
            {
                mInnerList.Clear();
                while (reader.Read())
                {
                    System.Diagnostics.Debug.WriteLine(reader.Name);
                    reader.Read();     //<Descriptor>
                    if (!reader.EOF)
                    {
                        Descriptor desc = Descriptor.ReadXmlS(reader);
                        if (desc != null)
                            mInnerList.Add(desc);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " : " + ex.InnerException.Message);
                throw ex;
            }
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }
        #endregion
    }
}
