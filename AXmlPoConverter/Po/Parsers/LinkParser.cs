using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AXmlPoConverter.Po.Parsers
{
	public class LinkParser : ParserBase
	{
		static Regex linkRegex = new Regex(@"^#:\s*(?<Value>.*)$");

		public LinkParser(string line) : base(line)
		{
		}

		protected override Regex ParserRegex
		{
			get
			{
				return linkRegex;
			}
		}

		protected override void Update(ParserContext context, string value)
		{
			context.Links.Add(this.Normalize(value));
		}
	}
}
