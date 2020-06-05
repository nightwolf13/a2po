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
		private string mappingPath;
		private Mapping mapping;

		public string AProjPath { get; set; }
		public string PoProjPath { get; set; }
		public ConvertionCmd Command { get; set; } = ConvertionCmd.Unknown;
		public bool IsBackup { get; set; } = false;
		public string MappingPath { get { return mappingPath; } set { mappingPath = value; mapping = new Mapping(value); } }
		public bool IgnoreASource { get; set; } = false;

		public Mapping Map { get 
			{ 
				if (mapping == null)
				{
					mapping = new Mapping();
				}
				return mapping; } }
	}
}
