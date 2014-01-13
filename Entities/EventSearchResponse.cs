using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventbriteNET.Entities
{
	public class EventSearchResponse : List<Event>
	{
		public EventbriteContext context;
		public EventSearchResponse(EventbriteContext context)
		{
			this.context = context;
		}

		public EventSearchSummary summary;
	}
}
