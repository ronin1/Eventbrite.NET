using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventbriteNET.Entities;
using EventbriteNET.Xml;

namespace EventbriteNET.HttpApi
{
	class EventSearchRequest : RequestBase
	{
		const string PATH = "event_search";
		public EventSearchRequest(EventSearchFilter request, EventbriteContext context) 
			: base(PATH, context)
		{
			if (request == null)
				throw new ArgumentNullException("request");

			IDictionary<string, string> query = request.ToParams();
			if (query != null && query.Count > 0)
			{
				foreach (var p in query)
					this.AddGet(p.Key, p.Value);
			}
		}

		public EventSearchResponse GetResponse()
		{
			return new EventSearchBuilder(this.Context).Build(base.GetResponse());
		}
	}
}
