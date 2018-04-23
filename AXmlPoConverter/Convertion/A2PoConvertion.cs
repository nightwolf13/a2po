﻿using AXmlPoConverter.AXml;
using AXmlPoConverter.Po;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXmlPoConverter.Convertion
{
	public class A2PoConvertion : ConvertionBase
	{
		public A2PoConvertion(ConvertionContext context) : base(context)
		{
		}

		public override void Convert()
		{
			DirectoryInfo aProjDir = new DirectoryInfo(Path.Combine(this.Context.AProjPath, AXmlHelper.AXML_PATH));

			FileInfo[] aFiles = aProjDir.GetFiles("strings.xml", SearchOption.AllDirectories);

			List<AXmlResource> aResources = new List<AXmlResource>();
			AXmlResource sourceRes = null;

			foreach (FileInfo aFile in aFiles)
			{
				AXmlResource resource = AXmlHelper.ReadAXml(aFile.FullName);
				if (resource != null)
				{
					Console.WriteLine($"{aFile.Directory.Name}/{aFile.Name} successfully read");
				}
				else
				{
					Console.WriteLine($"Error reading file {aFile.Directory.Name}/{aFile.Name}");
				}

				if (resource == null)
					continue;

				if (this.Context.Map.IsAIgnored(resource.Language))
				{
					Console.WriteLine($"{aFile.Directory.Name}/{aFile.Name} ignored");
					continue;
				}

				if (String.Equals(this.Context.Map.GetPo(resource.Language), "en", StringComparison.OrdinalIgnoreCase))
				{
					sourceRes = resource;
				}

				aResources.Add(resource);
			}

			// Create template.pot
			PoResource poFile = new PoResource();
			poFile.Language = "en";

			foreach (AXmlString aString in sourceRes)
			{
				if (aString.IsTranslatable)
				{
					PoString poString = poFile.FirstOrDefault(p => p.Id == aString.Value);

					if (poString == null)
					{
						poString = new PoString();
						poFile.Add(poString);
					}

					// Save id of android resource
					poString.Links.Add(aString.Name);
					poString.Id = aString.Value;
					poString.Value = "";
				}
			}
			this.MakeBackup("template.pot");
			poFile.Save(Path.Combine(this.Context.PoProjPath, "template.pot"));

			Console.WriteLine("template.pot converted");

			// Create po files

			foreach (AXmlResource aRes in aResources)
			{
				poFile = new PoResource();
				poFile.Language = this.Context.Map.GetPo(aRes.Language);

				foreach (AXmlString aString in aRes)
				{
					if (aString.IsTranslatable)
					{
						AXmlString sourceStr = sourceRes.FirstOrDefault(s => s.Name == aString.Name);
						string pid;
						if (sourceStr != null)
						{
							pid = sourceStr.Value;
						}
						else
						{
							pid = aString.Name;
						}

						PoString poString = poFile.FirstOrDefault(p => p.Id == pid);

						if (poString == null)
						{
							poString = new PoString();
							poFile.Add(poString);
						}

						poString.Id = pid;
						poString.Value = aString.Value;
						// Save id of android resource
						poString.Links.Add(aString.Name);
					}
				}

				this.MakeBackup(poFile.GetFileName());
				poFile.Save(Path.Combine(this.Context.PoProjPath, poFile.GetFileName()));
				Console.WriteLine($"{poFile.GetFileName()} converted");
			}
		}

		private void MakeBackup(string resName)
		{
			string fileName = Path.Combine(this.Context.PoProjPath, resName);
			if (this.Context.IsBackup)
			{
				if (File.Exists(fileName))
				{
					string tempFile = Path.Combine(this.Context.PoProjPath, "~" + resName);
					if (File.Exists(tempFile))
					{
						File.Delete(tempFile);
					}
					File.Move(fileName, tempFile);
				}
			}
		}
	}
}
