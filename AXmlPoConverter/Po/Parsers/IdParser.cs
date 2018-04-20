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
		static Regex idRegex = new Regex(@"^msgid\s*""(?<Value>.*)""$");

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
			//if (context.CurrentString != null)
			//{
			//	context.CurrentString.Comments.AddRange(context.Comments);
			//	context.PoResource.Add(context.CurrentString);
			//	context.NextString();
			//}

			//context.CurrentString = new PoString();
			context.CurrentString.Id = value;
			context.IsId = true;
			context.IsString = false;
		}
	}
}
