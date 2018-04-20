using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXmlPoConverter.Convertion
{
	public enum ConvertionCmd
	{
		Unknown,
		A2Po,
		Po2A,
		Clean
	}
	public class ConvertionContext
	{
		public string AProjPath { get; set; }
		public string PoProjPath { get; set; }
		public ConvertionCmd Command { get; set; } = ConvertionCmd.Unknown;
		public bool IsBackup { get; set; }
	}
}
