using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AXmlPoConverter.Po.Parsers
{
	public class StringParser : ParserBase
	{
		static Regex strRegex = new Regex($@"^msgstr\s*""(?<{GROUP_VALUE}>.*)""$");

		public StringParser(string line) : base(line)
		{
		}

		protected override Regex ParserRegex
		{
			get
			{
				return strRegex;
			}
		}

		protected override void Update(ParserContext context, string value)
		{
			context.IsId = false;
			context.IsString = true;

			context.CurrentString.Value = value.PoDeNormalize();
		}
	}
}
