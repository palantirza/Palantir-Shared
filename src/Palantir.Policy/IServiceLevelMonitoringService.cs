namespace Palantir.Policy
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Reporting;

	/// <summary>
	/// Monitors service levels.
	/// </summary>
    public interface IServiceLevelMonitoringService
    {
		/// <summary>
		/// Reports a failure on the indicated service.
		/// </summary>
		/// <param name="policyName">The policy name the service level is being reported to.</param>
		/// <param name="service">The service being reported against.</param>
		/// <param name="correlationId">The correlation ID for the service invocation. Optional.</param>
		/// <param name="timestamp">The timestamp that the service failed at.</param>
		void ReportFailure(string policyName, string service, string correlationId, long duration, DateTimeOffset timestamp);

		/// <summary>
		/// Reports successful execution of an indicated service.
		/// </summary>
		/// <param name="policyName">The policy name the service level is being reported to.</param>
		/// <param name="service">The service being reported against.</param>
		/// <param name="correlationId">The correlation ID for the service invocation. Optional.</param>
		/// <param name="duration">The duration, in milliseconds, of the service invocation.</param>
		/// <param name="timestamp">The timestamp that the service failed at.</param>
		void ReportSuccess(string policyName, string service, string correlationId, long duration, DateTimeOffset timestamp);

		/// <summary>
		/// Fetches a service level report.
		/// </summary>
		/// <returns>The service level report.</returns>
		Task<IDictionary<string, PolicyReport>> GetServiceLevelReport();

		/// <summary>
		/// Fetches a service report.
		/// </summary>
		/// <param name="policyName">The policy name.</param>
		/// <param name="service">The service name.</param>
		/// <returns>The service report.</returns>
		Task<ServiceReport> GetServiceReport(string policyName, string service);
	}
}
