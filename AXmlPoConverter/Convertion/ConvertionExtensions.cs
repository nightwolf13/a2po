using AXmlPoConverter.AXml;
using AXmlPoConverter.Po;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AXmlPoConverter.Convertion
{
	public static class ConvertionExtensions
	{
		static private Regex pluralNumberPattern = new Regex(@"@\d+");
		static private string NUMBER_PLACEHOLDER = "%d";

		private static string QuantityReplace(this string value, int number)
		{
			return value.Replace(NUMBER_PLACEHOLDER, "@" + number.ToString());
		}

		public static string GetPoValue(this AXmlPluralItem item)
		{
			switch (item.Quantity)
			{
				case QuantityType.zero:
					return item.Value.QuantityReplace(0);
				case QuantityType.one:
					return item.Value.QuantityReplace(1);
				case QuantityType.two:
					return item.Value.QuantityReplace(2);
				case QuantityType.few:
					return item.Value.QuantityReplace(3);
				case QuantityType.other:
					return item.Value.QuantityReplace(100);
				case QuantityType.many:
					return item.Value.QuantityReplace(59);
				default:
					return item.Value;
			}
		}

		public static string GetXmlValue(this PoString poString)
		{
			if (!poString.IsPluralString)
			{
				return poString.Value;
			}

			return pluralNumberPattern.Replace(poString.Value, NUMBER_PLACEHOLDER);
		}
	}
}
