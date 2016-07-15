namespace Palantir.Health
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// A policy builder for health monitoring.
	/// </summary>
	public sealed class HealthMonitoringPolicy
    {
		public readonly IReadOnlyList<IHealthIndicator> HealthIndicators;

		/// <summary>
		/// Initializes a new instance of the <see cref="HealthMonitoringPolicy"/> class.
		/// </summary>
		/// <param name="healthIndicators">The health indicators.</param>
		public HealthMonitoringPolicy(IEnumerable<IHealthIndicator> healthIndicators)
		{
			Contract.Requires(healthIndicators != null);

			HealthIndicators = healthIndicators.ToList().AsReadOnly();
		}

		/// <summary>
		/// Checks the health of the indicators within the indicated policy.
		/// </summary>
		/// <returns>The health report for the policy.</returns>
		public async Task<HealthReport> CheckHealthAsync()
		{
			var tasks = HealthIndicators.Select(x => x.CheckHealthAsync());

			await Task.WhenAll(tasks);

			return new HealthReport(tasks.Select(x => x.Result));
		}
	}
}