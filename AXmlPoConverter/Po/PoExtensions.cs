using AXmlPoConverter.AXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXmlPoConverter.Po
{
	public static class PoExtensions
	{
		public static IEnumerable<string> PoIterate(this string value)
		{
			string[] strLines = new string[] { "" };

			if (!string.IsNullOrEmpty(value))
				strLines = value.Split(new string[] { "\n" }, StringSplitOptions.None);

			for (int i = 0; i < strLines.Length; i++)
			{
				string strLine = strLines[i];

				if (strLine.Length >= 80)
				{
					string remLine = strLine;

					while (remLine.Length > 0)
					{
						string subLine = remLine.Substring(0, Math.Min(79, remLine.Length));
						remLine = remLine.Replace(subLine, "");

						string nChar = "";

						if (remLine.Length == 0 && (i != strLines.Length - 1))
						{
							nChar = "\\n";
						}

						yield return $@"""{subLine.PoNormalize()}{nChar}""";
					}
				}
				else
				{
					if (i == strLines.Length - 1)
					{
						yield return $@"""{strLine.PoNormalize()}""";
					} else
					{
						yield return $@"""{strLine.PoNormalize()}\n""";
					}
				}
			}
		}

		public static string PoNormalize(this string value)
		{
			if (string.IsNullOrEmpty(value))
				return value;

			return value.Replace("\\'", "'").Replace("\\", "\\\\");
			//.Replace("\\G", "\\\\G")
			//.Replace("\\-", "\\\\-").Replace("\\L", "\\\\L").Replace("\\\\K", "\\K").Replace("\\\\K", "\\K");//.Replace(@"\""", @"""").Replace("\\", "\\\\");
		}

		public static string PoDeNormalize(this string value)
		{
			if (string.IsNullOrEmpty(value))
				return value;

			return value.Replace("'", "\\'").Replace("\\\\", "\\");
			//.Replace("\\\\G", "\\G")
			//.Replace("\\\\-","\\-").Replace("\\\\L", "\\L").Replace("\\\\K", "\\K").Replace("\\\\K", "\\K");//.Replace(@"""", @"\""");
		}

		public static void WritePoTextLine(this StreamWriter streamWriter, string value, string label = null)
		{
			bool firstLine = true;
			foreach (string line in value.PoIterate())
			{
				string wLine = string.Empty;
				if (firstLine && !string.IsNullOrEmpty(label))
				{
					wLine = $@"{label} ";
				}

				wLine += line;
				firstLine = false;

				streamWriter.WriteLine(wLine);
			}
		}
	}
}
