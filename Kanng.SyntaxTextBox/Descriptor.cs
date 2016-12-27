using System;
using System.Drawing;
using System.Xml.Serialization;

namespace Kanng.SyntaxTextBox
{
    public enum DescriptorType
    {
        /// <summary>
        /// Causes the  of a single word
        /// </summary>
        Word,
        /// <summary>
        /// Causes the entire line from this point on the be ed, regardless of other tokens
        /// </summary>
        ToEOL,
        /// <summary>
        /// s all text until the end token;
        /// </summary>
        ToCloseToken
    }

    public enum DescriptorRecognition
    {
        /// <summary>
        /// Only if the whole token is equal to the word
        /// </summary>
        WholeWord,
        /// <summary>
        /// If the word starts with the token
        /// </summary>
        StartsWith,
        /// <summary>
        /// If the word contains the Token
        /// </summary>
        Contains
    }

    [Serializable()]
    public class Descriptor : ILoadDefaultData, IXmlSerializable
	{
        public string Token;
        public string CloseToken;
        public DescriptorType DescriptorType;
        public DescriptorRecognition DescriptorRecognition; 
        public bool UseForAutoComplete;
        public Color Color;
        public Font Font;
        
        public Descriptor()
        {
            //LoadDefaultData();
        }
		public Descriptor(string token, Color color, Font font, DescriptorType descriptorType, DescriptorRecognition dr, bool useForAutoComplete)
		{
			if (descriptorType == Kanng.SyntaxTextBox.DescriptorType.ToCloseToken)
			{
				throw new ArgumentException("You may not choose ToCloseToken DescriptorType without specifing an end token.");
			}
			Color = color;
			Font = font;
			Token = token;
			DescriptorType = descriptorType;
			DescriptorRecognition = dr;
			CloseToken = null;
			UseForAutoComplete = useForAutoComplete;
		}
		public Descriptor(string token, string closeToken, Color color, Font font, DescriptorType descriptorType, DescriptorRecognition dr, bool useForAutoComplete)
		{
			Color = color;
			Font = font;
			Token = token;
			DescriptorType = descriptorType;
			CloseToken = closeToken;
			DescriptorRecognition = dr;
			UseForAutoComplete = useForAutoComplete;
        }

        #region ILoadDefaultData 成员
        public void LoadDefaultData()
        {
            this.Color = System.Drawing.Color.Red;
            this.Font = new Font(new FontFamily("Arial"), 20);
            this.Token = "TestToken";
            this.CloseToken = "TestCloseToken";
            this.DescriptorType = DescriptorType.Word;
            this.DescriptorRecognition = DescriptorRecognition.WholeWord;
            this.UseForAutoComplete = true;
        }
        #endregion

        #region IXmlSerializable 成员
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            this.Font = (this.Font==null) ? new Font("Arial", 20) : Font;

            writer.WriteElementString("Token", this.Token);
            writer.WriteElementString("CloseToken", this.CloseToken);
            writer.WriteElementString("Color", Color.R.ToString() + "," + Color.G.ToString() + "," + Color.B.ToString());
            writer.WriteElementString("Font", this.Font.Name + "," + this.Font.Size.ToString());
            writer.WriteElementString("DescriptorType", this.DescriptorType.ToString());
            writer.WriteElementString("DescriptorRecognition", this.DescriptorRecognition.ToString());
            writer.WriteElementString("UseForAutoComplete", this.UseForAutoComplete.ToString());
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            try
            {
                string[] s;
                this.Token = reader.ReadElementString("Token");
                this.CloseToken = reader.ReadElementString("CloseToken");
                s = reader.ReadElementString("Color").Split(',');
                this.Color = Color.FromArgb(255, int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]));
                s = reader.ReadElementString("Font").Split(',');
                this.Font = new Font(s[0], Convert.ToInt32(s[1]));
                this.DescriptorType = (DescriptorType)Common.GetEnum(typeof(DescriptorType), reader.ReadElementString("DescriptorType"));
                this.DescriptorRecognition = (DescriptorRecognition)Common.GetEnum(typeof(DescriptorRecognition), reader.ReadElementString("DescriptorRecognition"));
                this.UseForAutoComplete = (reader.ReadElementString("UseForAutoComplete").ToLower()=="true") ? true : false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " : " + ex.InnerException.Message);
                LoadDefaultData();
            }
        }

        public static Descriptor ReadXmlS(System.Xml.XmlReader reader)
        {
            Descriptor desc = new Descriptor();
            try
            {
                string[] s;
                desc.Token = reader.ReadElementString("Token");
                desc.CloseToken = reader.ReadElementString("CloseToken");
                s = reader.ReadElementString("Color").Split(',');
                desc.Color = Color.FromArgb(255, int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]));
                s = reader.ReadElementString("Font").Split(',');
                desc.Font = null; //new Font(s[0], Convert.ToInt32(s[1]));
                desc.DescriptorType = (DescriptorType)Common.GetEnum(typeof(DescriptorType), reader.ReadElementString("DescriptorType"));
                desc.DescriptorRecognition = (DescriptorRecognition)Common.GetEnum(typeof(DescriptorRecognition), reader.ReadElementString("DescriptorRecognition"));
                desc.UseForAutoComplete = (reader.ReadElementString("UseForAutoComplete").ToLower()=="true") ? true : false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " : " + ex.InnerException.Message);
                return null;
            }
            return desc;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }
        #endregion

    }


}
