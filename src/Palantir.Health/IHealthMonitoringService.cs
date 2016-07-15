namespace Palantir.Health
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// A health monitoring service.
	/// </summary>
    public interface IHealthMonitoringService
    {
		/// <summary>
		/// Checks the health for a given policy.
		/// </summary>
		/// <returns>The health report.</returns>
		Task<HealthReport> CheckHealthAsync(string policyName);

		/// <summary>
		/// Fetches the most recent health check for a given policy.
		/// </summary>
		/// <returns>The health report.</returns>
		Task<HealthReport> FetchHealthAsync(string policyName);

		/// <summary>
		/// Checks the health for all policies.
		/// </summary>
		/// <returns>The health reports.</returns>
		Task<HealthReportCollection> CheckAllHealthAsync();
    }
}
