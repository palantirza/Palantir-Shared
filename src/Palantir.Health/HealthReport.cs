namespace Palantir.Health
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Contains the health monitoring report for a policy.
	/// </summary>
	public sealed class HealthReport
    {
		private readonly IReadOnlyList<Health> indicators;
		private readonly HealthStatus status;

		/// <summary>
		/// Initializes a new instance of the <see cref="HealthReport"/> class.
		/// </summary>
		/// <param name="indicators">The health indicators.</param>
		public HealthReport(IEnumerable<Health> indicators)
		{
			Contract.Requires(indicators != null);

			this.indicators = indicators.ToList().AsReadOnly();
			this.status = (from i in indicators
						  orderby i.Status descending
						  select i.Status).FirstOrDefault();
		}

		/// <summary>
		/// The health status. Unknown if no indicators were found.
		/// </summary>
		public HealthStatus Status => status;

		/// <summary>
		/// The health indicators.
		/// </summary>
		public IReadOnlyList<Health> Indicators => indicators;
    }
}
