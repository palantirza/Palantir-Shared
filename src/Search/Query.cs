namespace Palantir.Search
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Represents a parsed query.
	/// </summary>
    public sealed class Query
    {
		public Query()
		{
			Criteria = new MultiValueDictionary<string, string>();
		}

		public string Match { get; set; }
		
		public MultiValueDictionary<string, string> Criteria { get; }
    }
}
