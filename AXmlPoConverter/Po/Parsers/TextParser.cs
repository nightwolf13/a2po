using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AXmlPoConverter.Po.Parsers
{
	public class TextParser : ParserBase
	{
		static Regex textRegex = new Regex($@"^""(?<{GROUP_VALUE}>.*)""$");

		public TextParser(string line) : base(line)
		{
		}

		protected override Regex ParserRegex
		{
			get
			{
				return textRegex;
			}
		}

		protected override void Update(ParserContext context, string value)
		{
			string txt = Environment.NewLine + value.PoDeNormalize();
			if (context.IsId)
			{
				context.CurrentString.Id += txt;
			}
			else if (context.IsString)
			{
				context.CurrentString.Value += txt;
			}
			else
			{
				Console.WriteLine($"Unbound string found: {value}");
			}
		}
	}
}
