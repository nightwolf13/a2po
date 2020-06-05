using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AXmlPoConverter.Po.Parsers
{
	public class IdParser : ParserBase
	{
		static Regex idRegex = new Regex($@"^msgid\s*""(?<{GROUP_VALUE}>.*)""$");

		public IdParser(string line) : base(line)
		{
		}

		protected override Regex ParserRegex
		{
			get
			{
				return idRegex;
			}
		}

		protected override void Update(ParserContext context, string value)
		{
			context.CurrentString.Id = value.PoDeNormalize();
			context.IsId = true;
			context.IsString = false;
		}
	}
}
