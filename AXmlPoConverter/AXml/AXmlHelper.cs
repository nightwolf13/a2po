using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AXmlPoConverter.AXml
{
	public class AXmlHelper
	{
		public const string AXML_PATH = @"src\main\res";
		private const string N_RESOURCE = "resources";
		private const string N_STRING = "string";
		private const string A_NAME = "name";
		private const string A_TRANSLATABLE = "translatable";
		private const string N_PLURALS = "plurals";
		private const string N_ITEM = "item";
		private const string A_QUANTITY = "quantity";

		public static AXmlResource ReadAXml(string path)
		{
			AXmlResource res = null;
			DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(path));
			string language = "en";
			if (dir.Name.Contains("-"))
			{
				language = dir.Name.Substring(dir.Name.IndexOf("-") + 1);
			}

			using (XmlReader reader = XmlReader.Create(path, new XmlReaderSettings() { IgnoreComments = false }))
			{
				res = new AXmlResource();
				AXmlPlural xmlPlural = null;
				bool resourceContent = false;
				List<string> comments = new List<string>();
				while(reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Comment)
					{
						comments.Add(reader.Value.Trim());
						continue;
					}
					if (reader.IsStartElement())
					{
						if (reader.Name == N_RESOURCE)
						{
							resourceContent = true;
							continue;
						}
					}
					if (!resourceContent)
					{
						continue;
					}

					if (reader.NodeType == XmlNodeType.EndElement)
					{
						if (reader.Name == N_PLURALS)
						{
							xmlPlural = null;
							continue;
						} else if (reader.Name == N_RESOURCE)
						{
							resourceContent = false;
							continue;
						}
					}

					if (reader.IsStartElement())
					{
						if (reader.Name == N_STRING)
						{
							AXmlString xmlString = new AXmlString();
							xmlString.Name = reader.GetAttribute(A_NAME);
							if (comments.Count > 0)
							{
								xmlString.Comments.AddRange(comments);
								comments.Clear();
							}
							string trans = reader.GetAttribute(A_TRANSLATABLE);
							if (!string.IsNullOrEmpty(trans)) {
								xmlString.IsTranslatable = Convert.ToBoolean(trans);
							}
							xmlString.Value = reader.ReadElementContentAsString();
							res.Add(xmlString);
							continue;
						} else if (reader.Name == N_PLURALS)
						{
							xmlPlural = new AXmlPlural();
							xmlPlural.Name = reader.GetAttribute(A_NAME);
							if (comments.Count > 0)
							{
								xmlPlural.Comments.AddRange(comments);
								comments.Clear();
							}
							res.Add(xmlPlural);
							continue;
						} else if (reader.Name == N_ITEM)
						{
							if (xmlPlural == null)
							{
								continue;
							}

							AXmlPluralItem item = new AXmlPluralItem();
							item.Quantity = (QuantityType)Enum.Parse(typeof(QuantityType), reader.GetAttribute(A_QUANTITY));
							item.Value = reader.ReadElementContentAsString();

							xmlPlural.Add(item);
						}
					}
				}
			}

			if (res != null)
			{
				res.Language = language;
			}

			return res;
		}

		public static void SaveAXml(AXmlResource resource, string path)
		{
			string dir = Path.GetDirectoryName(path);

			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			using (XmlWriter writer = XmlWriter.Create(path, new XmlWriterSettings() { Indent = true }))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement(N_RESOURCE);

				foreach (AXmlResourceItem xmlItem in resource)
				{
					if (xmlItem.Comments != null && xmlItem.Comments.Count > 0)
					{
						foreach (string comment in xmlItem.Comments)
						{
							writer.WriteComment(comment);
						}
					}
					if (xmlItem is AXmlString)
					{
						AXmlString aString = (AXmlString)xmlItem;
						writer.WriteStartElement(N_STRING);

						writer.WriteAttributeString(A_NAME, aString.Name);
						if (!aString.IsTranslatable)
						{
							writer.WriteAttributeString(A_TRANSLATABLE, Convert.ToString(aString.IsTranslatable));
						}

						writer.WriteValue(aString.Value);
						writer.WriteEndElement();
					} else if (xmlItem is AXmlPlural)
					{
						writer.WriteStartElement(N_PLURALS);

						writer.WriteAttributeString(A_NAME, xmlItem.Name);

						foreach (AXmlPluralItem item in ((AXmlPlural)xmlItem).Items.Values)
						{
							if (string.IsNullOrEmpty(item.Value))
								continue;

							writer.WriteStartElement(N_ITEM);
							writer.WriteAttributeString(A_QUANTITY, Enum.GetName(typeof(QuantityType), item.Quantity));
							writer.WriteValue(item.Value);

							writer.WriteEndElement();
						}

						writer.WriteEndElement();
					}
				}

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
		}
	}
}
