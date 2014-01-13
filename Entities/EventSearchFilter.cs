using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace EventbriteNET.Entities
{
	public class EventSearchFilter
	{
		/// <summary>
		/// The search keywords. To run an OR search, you need this format: “keywords=google%20OR%20multimedia”
		/// </summary>
		public string keywords;
		/// <summary>
		/// Event categories (comma seperated): conferences, conventions, entertainment, fundraisers, meetings, other, performances, reunions, sales, seminars, social, sports, tradeshows, travel, religion, fairs, food, music, recreation.
		/// </summary>
		public readonly HashSet<string> category = new HashSet<string>();
		/// <summary>
		/// The venue address.
		/// </summary>
		public string address;
		/// <summary>
		/// The venue city.
		/// </summary>
		public string city;
		/// <summary>
		/// The venue state/province/county/territory depending on the country. 2-letter state code is required for US addresses.
		/// </summary>
		public string region;
		/// <summary>
		/// The postal/zip code of the venue.
		/// </summary>
		public string postal_code;
		/// <summary>
		/// 2-letter country code, according to the ISO 3166 format.
		/// </summary>
		public string country;
		/// <summary>
		/// The event start date. Limit the list of results to a date range, specified by a label or by exact dates. Currently supported labels include: “All”, “Future”, “Past”, “Today”, “Yesterday”, “Last Week”, “This Week”, “Next week”, “This Month”, “Next Month” and months by name, e.g. “October”. Exact date ranges take the form “YYYY-MM-DD YYYY-MM-DD”, e.g. “2008-04-25 2008-04-27″.
		/// </summary>
		public string date;
		/// <summary>
		/// The organizer name.
		/// </summary>
		public string organizer;

		const int LIMIT = 100;
		int _lim = LIMIT;
		/// <summary>
		/// Limit the number of events returned. Maximum limit is 100 events per page. Default is 10.
		/// </summary>
		public int max
		{
			get { return _lim; }
			set
			{
				if (value > LIMIT)
					_lim = value;
				else if (value < 1)
					_lim = 1;
				else
					_lim = value;
			}
		}

		/// <summary>
		/// Sort the list of events by “id”, “date”, “name”, “city”. The default is “date”.
		/// </summary>
		public EventSearchSortBy sort_by;

		int _pg = 1;
		/// <summary>
		/// Allows for paging through the results of a query. Default is 1.
		/// </summary>
		public int page
		{
			get { return _pg; }
			set
			{
				if (value < 1)
					_pg = 1;
				else
					_pg = value;
			}
		}

		/// <summary>
		/// Returns events with id greater than “since_id” value. Default is 1.
		/// </summary>
		public long since_id;

		readonly HashSet<string> _ignores = new HashSet<string>
		{
			//"sort_by",
			"category",
		};
		void ForEachProperty(Action<PropertyInfo> prop)
		{
			if (prop == null)
				throw new ArgumentNullException("prop");

			PropertyInfo[] pInfo = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance);
			foreach (PropertyInfo propInfo in pInfo)
			{
				if (_ignores.Contains(propInfo.Name))
					continue;

				prop(propInfo);
			}
		}

		public IDictionary<string, string> ToParams()
		{
			var map = new Dictionary<string, string>();
			if (category.Count > 0)
			{
				var sb = new StringBuilder();
				foreach (string v in category)
				{
					sb.Append(v);
					sb.Append(',');
				}
				map.Add("category", sb.ToString());
			}
			ForEachProperty(p =>
			{
				object v = p.GetValue(this, null);
				string s;
				if (v != null && !string.IsNullOrWhiteSpace(s = v.ToString()))
					map.Add(p.Name, s);
			});
			return map;
		}

	}
}
