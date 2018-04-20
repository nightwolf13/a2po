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
					aXmlPath = Path.Combine(aResPath, "values\\strings.xml");

					if (File.Exists(aXmlPath))
					{
						aRes = AXmlHelper.ReadAXml(aXmlPath);
					}
				}
				else
				{
					aXmlPath = Path.Combine(aResPath, $"values-{poRes.Language}\\strings.xml");

					if (Directory.Exists(aResPath))
					{
						string pattern = $"values*-{poRes.Language}";
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

				aRes.Language = poRes.Language;

				foreach (PoString poStr in poRes)
				{
					string id = poStr.Id;
					PoString tempStr = tempPot.FirstOrDefault(s => s.Id == poStr.Id);
					if (tempStr != null && !string.IsNullOrEmpty(tempStr.Link))
					{
						id = tempStr.Link;
					}
					else if (!string.IsNullOrEmpty(poStr.Link))
					{
						id = poStr.Link;
					}
					else
					{
						Console.WriteLine($"WARNING: string key not found. File: {poFile.Name}, Res: {poStr.Id}");
						id = poStr.Id;
					}

					AXmlString aString = aRes.FirstOrDefault(a => a.Name == id);

					if (aString == null)
					{
						aString = new AXmlString();
						aRes.Add(aString);
					}

					aString.Name = id;
					aString.Value = poStr.Value;
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
