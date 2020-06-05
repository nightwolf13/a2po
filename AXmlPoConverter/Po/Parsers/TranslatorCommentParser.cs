﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AXmlPoConverter.Po.Parsers
{
	public class TranslatorCommentParser : ParserBase
	{
		static Regex commentRegex = new Regex($@"^#\s+(?<{GROUP_VALUE}>.*)$");

		public TranslatorCommentParser(string line) : base(line)
		{
		}

		protected override Regex ParserRegex
		{
			get
			{
				return commentRegex;
			}
		}

		protected override void Update(ParserContext context, string value)
		{
			context.Comments.Add(value.PoDeNormalize());
		}
	}
}
