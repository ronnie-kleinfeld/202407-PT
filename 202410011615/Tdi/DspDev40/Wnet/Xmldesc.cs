//the class received from Wondernet in 01.08
using System;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace XmlDesc
{
	/// <summary>
	/// Summary description for EncoderDecoder.
	/// </summary>
	public class EncoderDecoder
	{
		string Path="";
		public EncoderDecoder(string PathOfXml)
		{
			try
			{
				Path = PathOfXml;
			}
			catch(Exception ex)
			{
				Trace.WriteLine(ex.Message);
			}
		}
		public string UpdateDescriptionFile(string DescriptionField)
		{
			string Res = "";
			string ValNum="";
			try
			{
				if(!File.Exists(Path))
					throw new FileNotFoundException();

				XmlDocument doc = new XmlDocument();
				doc.Load(Path);
				XmlNodeList nl = doc.GetElementsByTagName("Desc");
//				if(nl.Count>100)
//				{
//					File.Delete(Path);
//					//create a new xml file
//					XmlTextWriter xmlWriter = new XmlTextWriter(Path, System.Text.Encoding.UTF8);
//					xmlWriter.Formatting = Formatting.Indented;
//					xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
//					xmlWriter.WriteStartElement("Descriptions");
//					xmlWriter.WriteEndElement();
//					xmlWriter.Close();
//					doc.Load(Path);
//					ValNum = "1";
//					XmlNode nodeDescriptionNum	= doc.CreateElement("Desc");
//					XmlNode nodeField	= doc.CreateElement("fd");
//					nodeField.InnerText = DescriptionField;
//					XmlAttribute Name =  doc.CreateAttribute("name");
//					Name.Value=ValNum;
//					nodeDescriptionNum.Attributes.Append(Name);
//					nodeDescriptionNum.AppendChild(nodeField);		
//					doc.LastChild.AppendChild(nodeDescriptionNum);
//					doc.Save(Path);
//					Res=Name.Value;
//				}
//				else
//				{
					ValNum =  ((int)(nl.Count + 1)).ToString();
					XmlNode nodeDescriptionNum	= doc.CreateElement("Desc");
					XmlNode nodeField	= doc.CreateElement("fd");
					nodeField.InnerText = DescriptionField;
					XmlAttribute Name =  doc.CreateAttribute("name");
					Name.Value=ValNum;
					nodeDescriptionNum.Attributes.Append(Name);
					nodeDescriptionNum.AppendChild(nodeField);
					doc.LastChild.AppendChild(nodeDescriptionNum);
					doc.Save(Path);
					Res=Name.Value;
//				}
			}
			catch(Exception Ex)
			{
				Trace.WriteLine(Ex.Message);
				return null;
			}
			return "$$"+Res;
		}
	}
}
