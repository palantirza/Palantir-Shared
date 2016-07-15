namespace Palantir.Health
{
	using Microsoft.Extensions.Caching.Memory;
	using Microsoft.Extensions.Logging;
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// A default implementation of the health monitoring service.
	/// </summary>
	public sealed class DefaultHealthMonitoringService : IHealthMonitoringService
    {
		private ILogger logger;
		private IHealthMonitoringPolicyProvider policyProvider;
		private readonly IMemoryCache memoryCache;
		private readonly MemoryCacheEntryOptions options = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = new TimeSpan(0, 0, 0, 10) };

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultHealthMonitoringService"/>.
		/// </summary>
		/// <param name="loggerFactory">The logger.</param>
		/// <param name="policyProvider">The policy provider.</param>
		public DefaultHealthMonitoringService(ILoggerFactory loggerFactory, IHealthMonitoringPolicyProvider policyProvider, IMemoryCache memoryCache)
		{
			Contract.Requires(loggerFactory != null);
			Contract.Requires(policyProvider != null);
			Contract.Requires(memoryCache != null);

			this.logger = loggerFactory.CreateLogger<DefaultHealthMonitoringService>();
			this.policyProvider = policyProvider;
			this.memoryCache = memoryCache;
		}

		/// <summary>
		/// Checks the health for all policies.
		/// </summary>
		/// <returns>The health reports.</returns>
		public async Task<HealthReportCollection> CheckAllHealthAsync()
		{
			var reports = from p in await policyProvider.GetAllPoliciesAsync()
						  orderby p.Key
						  select new { PolicyName = p.Key, HealthReport = p.Value.CheckHealthAsync() };

			await Task.WhenAll(reports.Select(x => x.HealthReport));
			
			var result = new HealthReportCollection(reports.ToDictionary(x => x.PolicyName, x => x.HealthReport.Result));
			foreach (var report in result.PolicyReports)
				memoryCache.Set<HealthReport>(report.Key, report.Value, options);

			return result;
		}

		/// <summary>
		/// Checks the health of the indicators within the indicated policy.
		/// </summary>
		/// <param name="policyName">The policy name.</param>
		/// <returns>The health report for the policy name.</returns>
		public async Task<HealthReport> CheckHealthAsync(string policyName)
		{
			Contract.Requires(policyName != null);

			var policy = await policyProvider.GetPolicyAsync(policyName);
			if (policy == null)
				throw new InvalidOperationException($"No policy found: {policyName}.");

			var report = await policy.CheckHealthAsync();

			memoryCache.Set<HealthReport>(policyName, report, options);
			
			return report;
		}

		/// <summary>
		/// Fetches the most recent health check for a given policy.
		/// </summary>
		/// <returns>The health report.</returns>
		public async Task<HealthReport> FetchHealthAsync(string policyName)
		{
			Contract.Requires(policyName != null);

			return memoryCache.Get<HealthReport>(policyName) ?? await CheckHealthAsync(policyName);
		}
	}
}
