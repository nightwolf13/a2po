using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXmlPoConverter.Convertion
{
	public abstract class ConvertionBase
	{
		protected ConvertionContext Context { get; private set; }
		public ConvertionBase(ConvertionContext context)
		{
			this.Context = context;
		}

		public abstract void Convert();
	}
}
