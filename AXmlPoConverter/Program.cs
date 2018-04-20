using AXmlPoConverter.AXml;
using AXmlPoConverter.Convertion;
using AXmlPoConverter.Po;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AXmlPoConverter
{
	class Program
	{
		static Regex cmdPattern = new Regex(@"^-(?<Cmd>\w+)(:(?<Value>.*))?$");
		static void Main(string[] args)
		{
			ConvertionContext context = new ConvertionContext();

			if (!prepareArgs(args, context))
			{
				return;
			}

			switch (context.Command)
			{
			case ConvertionCmd.Unknown:
				Console.WriteLine("Unknown command line arguments. See help (-h)");
				break;
			case ConvertionCmd.A2Po:
				A2PoConvertion a2po = new A2PoConvertion(context);
				a2po.Convert();
				break;
			case ConvertionCmd.Po2A:
				Po2AConvertion po2a = new Po2AConvertion(context);
				po2a.Convert();
				break;
			case ConvertionCmd.Clean:
				cleanFolders(context);
				break;
			default:
				break;
			}

			Console.WriteLine("Done");
		}

		private static void cleanFolders(ConvertionContext context)
		{
			DirectoryInfo aProjDir = new DirectoryInfo(Path.Combine(context.AProjPath, AXmlHelper.AXML_PATH));
			FileInfo[] aFiles = aProjDir.GetFiles("~strings.xml", SearchOption.AllDirectories);

			foreach (FileInfo aFile in aFiles)
			{
				try
				{
					aFile.Delete();
					Console.WriteLine($"File {aFile.Directory.Name}/{aFile.Name} deleted");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Cannot remove file {aFile.Directory.Name}/{aFile.Name}: {ex.Message}");
				}
			}

			DirectoryInfo poProjDir = new DirectoryInfo(context.PoProjPath);
			FileInfo[] poFiles = poProjDir.GetFiles("~*.po*");

			foreach (FileInfo pFile in poFiles)
			{
				try
				{
					pFile.Delete();
					Console.WriteLine($"File {pFile.Name} deleted");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Cannot remove file {pFile.Name}: {ex.Message}");
				}
			}
		}

		private static bool prepareArgs(string[] args, ConvertionContext context)
		{
			try
			{
				foreach (string arg in args)
				{
					if (arg == "-h")
					{
						Console.WriteLine(Resource.Help);
						return false;
					}

					string[] cmdData = splitCommand(arg);

					if (cmdData.Length > 0)
					{
						switch (cmdData[0])
						{
						case "x":
							context.AProjPath = cmdData[1];
							break;
						case "p":
							context.PoProjPath = cmdData[1];
							break;
						case "a":
							switch (cmdData[1])
							{
							case "x2p":
								context.Command = ConvertionCmd.A2Po;
								break;
							case "p2x":
								context.Command = ConvertionCmd.Po2A;
								break;
							case "clean":
								context.Command = ConvertionCmd.Clean;
								break;
							}
							break;
						case "t":
							context.IsBackup = true;
							break;
						case "m":
							context.MappingPath = cmdData[1];
							break;
						case "is":
							context.IgnoreASource = true;
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
				return false;
			}

			return true;
		}

		private static string[] splitCommand(string arg)
		{
			Match cmdM = cmdPattern.Match(arg);

			if (cmdM.Success)
			{
				return new string[] { cmdM.Groups["Cmd"].Value, cmdM.Groups["Value"].Value };
			}
			else
			{
				throw new Exception("Invalid Command arguments");
			}
		}
	}
}
