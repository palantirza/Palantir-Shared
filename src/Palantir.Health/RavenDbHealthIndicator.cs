namespace Palantir.Health
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// A healt indicator for RavenDB.
	/// </summary>
	public class RavenDbHealthIndicator : IHealthIndicator
	{
		private readonly string url;
		private readonly string database;

		/// <summary>
		/// Initializes a new instance of the <see cref="RavenDbHealthIndicator"/> class.
		/// </summary>
		/// <param name="url">The RavenDB Url.</param>
		/// <param name="database">The RavenDB database.</param>
		public RavenDbHealthIndicator(string url, string database)
		{
			Contract.Requires(!string.IsNullOrEmpty(url));
			Contract.Requires(!string.IsNullOrEmpty(database));

			this.url = url;
			this.database = database;
		}

		/// <summary>
		/// The RavenDB URL.
		/// </summary>
		public string Url => url;

		/// <summary>
		/// Peforms a health check.
		/// </summary>
		/// <returns>The health.</returns>
		public Task<Health> CheckHealthAsync()
		{
			throw new NotImplementedException();
		}
	}
}
