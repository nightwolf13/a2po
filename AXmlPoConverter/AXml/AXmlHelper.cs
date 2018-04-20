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
		public static AXmlResource ReadAXml(string path)
		{
			AXmlResource res = null;
			DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(path));
			string language = "en";
			if (dir.Name.Contains("-"))
			{
				language = dir.Name.Substring(dir.Name.IndexOf("-") + 1);
			}

			using (XmlReader reader = XmlReader.Create(path))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(AXmlResource));

				res = xmlSerializer.Deserialize(reader) as AXmlResource;
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
				XmlSerializer serialize = new XmlSerializer(typeof(AXmlResource));

				serialize.Serialize(writer, resource);
			}
		}
	}
}
