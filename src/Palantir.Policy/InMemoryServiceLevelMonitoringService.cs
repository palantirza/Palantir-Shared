namespace Palantir.Policy
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using HdrHistogram;
	using Reporting;
	using Microsoft.Extensions.Options;
	using System.Diagnostics.Contracts; 

	/// <summary>
	/// An in memory implementation of the <see cref="IServiceLevelMonitoringService"/> interface.
	/// </summary>
	public sealed class InMemoryServiceLevelMonitoringService : IServiceLevelMonitoringService
	{
		private Dictionary<string, PolicyCounter> policies;
		private IReadOnlyDictionary<string, Predicate<ServiceLevelRecord>> policyDefinitions;

		/// <summary>
		/// Initializes a new instance of the <see cref="InMemoryServiceLevelMonitoringService"/> class.
		/// </summary>
		/// <param name="options">The service level options.</param>
		public InMemoryServiceLevelMonitoringService(IOptions<ServiceLevelOptions> options)
		{
			Contract.Requires(options != null);

			this.policies = options.Value.GetAllPolicies().ToDictionary(x => x.Key, x => new PolicyCounter());
			this.policyDefinitions = options.Value.GetAllPolicies();
		}

		/// <summary>
		/// Fetches a service level report.
		/// </summary>
		/// <returns>The service level report.</returns>
		public Task<IDictionary<string, PolicyReport>> GetServiceLevelReport()
		{
			var reports = (from p in policies
						  select new KeyValuePair<string, PolicyReport>(p.Key,
						  new PolicyReport(
							  policyDefinitions[p.Key],
							  p.Value.Services.ToDictionary(x => x.Key, x => x.Value.GetRecord())
						  ))).ToDictionary(x => x.Key, x => x.Value);

			return Task.FromResult<IDictionary<string, PolicyReport>>(reports);
		}

		/// <summary>
		/// Fetches a service report.
		/// </summary>
		/// <param name="policyName">The policy to fetch the report for.</param>
		/// <param name="service">The service to fetch the report for.</param>
		/// <returns>The service report.</returns>
		public Task<ServiceReport> GetServiceReport(string policyName, string service)
		{
			var report = new ServiceReport(policyDefinitions[policyName], policies[policyName].Services[service].GetRecord());

			return Task.FromResult(report);
		}

		/// <summary>
		/// Reports a failure on the indicated service.
		/// </summary>
		/// <param name="policyName">The policy name the service level is being reported to.</param>
		/// <param name="service">The service being reported against.</param>
		/// <param name="correlationId">The correlation ID for the service invocation. Optional.</param>
		/// <param name="timestamp">The timestamp that the service failed at.</param>
		public void ReportFailure(string policyName, string service, string correlationId, long duration, DateTimeOffset timestamp)
		{
			if (!policies.ContainsKey(policyName))
				throw new InvalidOperationException($"Unknown service level policy {policyName}");

			policies[policyName].ReportFailure(service, duration, timestamp);
		}

		/// <summary>
		/// Reports successful execution of an indicated service.
		/// </summary>
		/// <param name="policyName">The policy name the service level is being reported to.</param>
		/// <param name="service">The service being reported against.</param>
		/// <param name="correlationId">The correlation ID for the service invocation. Optional.</param>
		/// <param name="duration">The duration, in milliseconds, of the service invocation.</param>
		/// <param name="timestamp">The timestamp that the service failed at.</param>
		public void ReportSuccess(string policyName, string service, string correlationId, long duration, DateTimeOffset timestamp)
		{
			if (!policies.ContainsKey(policyName))
				throw new InvalidOperationException($"Unknown service level policy {policyName}");

			policies[policyName].ReportSuccess(service, duration, timestamp);
		}
	}
}
