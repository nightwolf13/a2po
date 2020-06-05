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
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace AXmlPoConverter.AXml
{
	[DebuggerDisplay("{Language}")]
	public class AXmlResource : List<AXmlResourceItem>
	{
		public string Language { get; set; }
	}

	public abstract class AXmlResourceItem
	{
		public string Name { get; set; }
	}

	[DebuggerDisplay("{Name} - {Value}")]
	public class AXmlString : AXmlResourceItem
	{
		public bool IsTranslatable { get; set; } = true;
		public string Value { get; set; }
	}

	[DebuggerDisplay("{Name}")]
	public class AXmlPlural : AXmlResourceItem, IXmlSerializable
	{
		public Dictionary<QuantityType, AXmlPluralItem> Items { get; set; } = new Dictionary<QuantityType, AXmlPluralItem>();

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			if (reader.IsStartElement())
			{
				while (reader.Read())
				{
					if (reader.Name == "item")
					{
						XmlSerializer xmlSerializer = new XmlSerializer(typeof(AXmlPluralItem));
						AXmlPluralItem item = (AXmlPluralItem)xmlSerializer.Deserialize(reader);
						this.Items.Add(item.Quantity, item);
					}
				}
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			throw new NotImplementedException();
		}

		public void Add(AXmlPluralItem item)
		{
			this.Items.Add(item.Quantity, item);
		}
	}


	[DebuggerDisplay("{Quantity} - {Value}")]
	public class AXmlPluralItem
	{
		public QuantityType Quantity { get; set; }
		public string Value { get; set; }
	}
}
