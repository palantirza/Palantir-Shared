namespace Palantir.Search
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;

	/// <summary>
	/// A parser which parses a text query.
	/// </summary>
	/// <typeparam name="TTarget">The target type.</typeparam>
	public sealed class QueryParser
    {
		/// <summary>
		/// Parses the query.
		/// </summary>
		/// <param name="queryText">The query text.</param>
		/// <returns>The parsed query.</returns>
		public Query Parse(string queryText)
		{
			if (string.IsNullOrEmpty(queryText))
			{
				return new Query();
			}

			var regex = new Regex(@"(?<query>\w+)(\s(?<field>\w+)\:(?<value>(\w+|\{\w+(,\w+)*\})))*", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
			var query = regex.Match(queryText);

			var result = new Query
			{
				Match = query.Groups["query"].Value
			};

			for (int i = 0; i < query.Groups["field"].Captures.Count; i++)
			{
				var field = query.Groups["field"].Captures[i].Value;
				var value = query.Groups["value"].Captures[i].Value;

				result.Criteria.Add(field, value);
			}

			return result;
		}
	}
}
