using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXmlPoConverter.Po
{
	public class ParserContext
	{
		public bool IsId { get; set; }
		public bool IsString { get; set; }
		public List<string> Comments { get; private set; }
		public List<string> Links { get; set; }
		public PoString CurrentString { get; set; }
		public PoResource PoResource { get; private set; }

		public ParserContext(PoResource res)
		{
			this.PoResource = res;
			this.IsId = false;
			this.IsString = false;
			this.NextString();
		}

		public void NextString()
		{
			if (this.CurrentString != null)
			{
				if (!string.IsNullOrEmpty(this.CurrentString.Id))
				{
					this.PoResource.Add(this.CurrentString);
				}
				this.CurrentString.Comments.AddRange(this.Comments);
				this.CurrentString.Links.AddRange(this.Links);
			}
			this.CurrentString = new PoString();
			this.Comments = new List<string>();
			this.Links = new List<string>();
		}

		public void FinalizeResource()
		{
			if (this.CurrentString != null)
			{
				this.CurrentString.Comments.AddRange(this.Comments);
				this.CurrentString.Links.AddRange(this.Links);
				this.PoResource.Add(this.CurrentString);
			}
		}
	}
}
