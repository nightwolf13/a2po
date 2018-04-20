using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AXmlPoConverter.AXml
{
	[XmlType(TypeName = "resources")]
	[DebuggerDisplay("{Language}")]
	public class AXmlResource :  List<AXmlString>
	{
		[XmlIgnore]
		public string Language { get; set; }
	}

	[Serializable]
	[XmlType(TypeName = "string")]
	[DebuggerDisplay("{Name} - {Value}")]
	public class AXmlString
	{
		[XmlAttribute(AttributeName = "name", DataType = "string")]
		public string Name { get; set; }
		[DefaultValue(true), XmlAttribute(AttributeName = "translatable", DataType = "boolean")]
		public bool IsTranslatable { get; set; } = true;
		[XmlText]
		public string Value { get; set; }
	}
}
