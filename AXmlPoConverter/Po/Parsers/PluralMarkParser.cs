using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AXmlPoConverter.Po.Parsers
{
	public class PluralMarkParser: ParserBase
	{
		static readonly string POSSIBLE_VALUES = Enum.GetNames(typeof(QuantityType)).Aggregate((e1, e2) => $"{e1}|{e2}");

		static Regex pluralRegex = new Regex($@"^#\.\s+(?<{GROUP_VALUE}>({POSSIBLE_VALUES}))\s*$");

		public PluralMarkParser(string line) : base(line)
		{
		}

		protected override Regex ParserRegex
		{
			get
			{
				return pluralRegex;
			}
		}

		protected override void Update(ParserContext context, string value)
		{
			context.PluralType = (QuantityType)Enum.Parse(typeof(QuantityType), value);
		}
	}
}
