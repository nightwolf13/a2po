using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AXmlPoConverter.Po.Parsers
{
	public abstract class ParserBase
	{
		protected string Line { get; private set; }
		public ParserBase(string line)
		{
			this.Line = line;
		}

		protected abstract Regex ParserRegex { get; }
		protected abstract void Update(ParserContext context, string value);

		public bool Parse(ParserContext context)
		{
			Match m = this.ParserRegex.Match(this.Line);

			if (m.Success)
			{
				Update(context, m.Groups["Value"].Value);
				return true;
			}

			return false;
		}

		protected string Normalize(string value)
		{
			if (string.IsNullOrEmpty(value))
				return value;

			return value.Replace("'", "\\'").Replace("\\\\", "\\");
		}
	}
}
