using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXmlPoConverter.Convertion
{
	public class Mapping
	{
		private Dictionary<string, string> a2po = new Dictionary<string, string>();
		private Dictionary<string, string> po2a = new Dictionary<string, string>();

		public Mapping(string path)
		{
			this.Load(path);
		}

		private void Load(string path)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();

					if (string.IsNullOrWhiteSpace(line))
						continue;

					var pair = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

					if (pair.Length < 2)
						continue;

					a2po.Add(pair[0], pair[1]);
					po2a.Add(pair[1], pair[0]);
				}
			}
		}

		public string GetPo(string a)
		{
			if (a2po.ContainsKey(a))
				return a2po[a];

			return a;
		}

		public string GetA(string po)
		{
			if (po2a.ContainsKey(po))
				return po2a[po];

			return po;
		}
	}
}
