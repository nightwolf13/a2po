using AXmlPoConverter.AXml;
using AXmlPoConverter.Po;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXmlPoConverter.Convertion
{
	public class Po2AConvertion : ConvertionBase
	{
		public Po2AConvertion(ConvertionContext context) : base(context)
		{
		}

		public override void Convert()
		{
			DirectoryInfo dInfo = new DirectoryInfo(this.Context.PoProjPath);
			string aResPath = Path.Combine(this.Context.AProjPath, AXmlHelper.AXML_PATH);

			string tempPotPath = Path.Combine(dInfo.FullName, "template.pot");

			PoResource tempPot = PoResource.ReadPoFile(tempPotPath);

			var poFiles = dInfo.GetFiles("*.po").Where(f => !f.Name.StartsWith("~"));

			foreach (FileInfo poFile in poFiles)
			{
				PoResource poRes = PoResource.ReadPoFile(poFile.FullName);

				if (poRes == null)
				{
					Console.WriteLine($"File {poFile.Name} didn't load");
					continue;
				}

				AXmlResource aRes = null;
				string aXmlPath = null;

				if (string.Equals(poRes.Language, "en", StringComparison.OrdinalIgnoreCase))
				{
					if (this.Context.IgnoreASource)
					{
						Console.WriteLine("values/string.xml is skipped");
						continue;
					}

					aXmlPath = Path.Combine(aResPath, "values\\strings.xml");

					if (File.Exists(aXmlPath))
					{
						aRes = AXmlHelper.ReadAXml(aXmlPath);
					}
				}
				else
				{
					aXmlPath = Path.Combine(aResPath, $"values-{this.Context.Map.GetA(poRes.Language)}\\strings.xml");

					if (Directory.Exists(aResPath))
					{
						string pattern = $"values*-{this.Context.Map.GetA(poRes.Language)}";
						DirectoryInfo[] dirs = new DirectoryInfo(aResPath).GetDirectories(pattern, SearchOption.TopDirectoryOnly);

						if (dirs.Length > 0)
						{
							aXmlPath = Path.Combine(dirs[0].FullName, "strings.xml");

							if (File.Exists(aXmlPath))
							{
								aRes = AXmlHelper.ReadAXml(aXmlPath);
							}
						}
					}
				}

				if (aRes == null)
					aRes = new AXmlResource();

				aRes.Language = this.Context.Map.GetA(poRes.Language);

				foreach (PoString poStr in poRes)
				{
					PoString tempStr = tempPot.FirstOrDefault(s => s.Id == poStr.Id);
					var links = new List<string>();

					if (tempStr != null && tempStr.Links != null)
					{
						links.AddRange(tempStr.Links);
					}

					if (poStr.Links != null)
					{
						links.AddRange(poStr.Links.Where(l => !links.Contains(l)));
					}

					if (links.Count == 0)
					{
						Console.WriteLine($"WARNING: string key not found. File: {poFile.Name}, Res: {poStr.Id}");
						links.Add(poStr.Id);
					}
					
					foreach (string id in links)
					{
						AXmlString aString = aRes.FirstOrDefault(a => a.Name == id);

						if (aString == null)
						{
							aString = new AXmlString();
							aRes.Add(aString);
						}

						aString.Name = id;
						aString.Value = poStr.Value;
					}
				}

				FileInfo ax = new FileInfo(aXmlPath);
				this.MakeBackup(aXmlPath);
				try
				{
					AXmlHelper.SaveAXml(aRes, aXmlPath);
					Console.WriteLine($"{ax.Directory.Name}/{ax.Name} converted.");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error saving file: {ax.Directory.Name}/{ax.Name}. Message: {ex.Message}");
				}
			}
		}

		private void MakeBackup(string filePath)
		{
			if (this.Context.IsBackup)
			{
				if (File.Exists(filePath))
				{
					string tempFilePath = Path.Combine(Path.GetDirectoryName(filePath), "~" + Path.GetFileName(filePath));

					if (File.Exists(tempFilePath))
					{
						File.Delete(tempFilePath);
					}
					File.Move(filePath, tempFilePath);
				}
			}
		}
	}
}
