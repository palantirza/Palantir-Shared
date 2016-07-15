namespace Palantir.Policy
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Represents the service levels for a policy.
	/// </summary>
	internal sealed class PolicyCounter
    {
		private ConcurrentDictionary<string, ServiceCounter> services = new ConcurrentDictionary<string, ServiceCounter>();

		/// <summary>
		/// Reports the success duration for the operation.
		/// </summary>
		/// <param name="serviceName">The service to report against.</param>
		/// <param name="duration">The duration to report.</param>
		/// <param name="timestamp">The timestamp.</param>
		/// <returns>The policy service level.</returns>
		public PolicyCounter ReportSuccess(string serviceName, long duration, DateTimeOffset timestamp)
		{
			services.AddOrUpdate(serviceName, new ServiceCounter().ReportSuccess(duration, timestamp), (k, s) => s.ReportSuccess(duration, timestamp));

			return this;
		}

		/// <summary>
		/// Reports the failure duration for the operation.
		/// </summary>
		/// <param name="serviceName">The service to report against.</param>
		/// <param name="duration">The duration to report.</param>
		/// <param name="timestamp">The timestamp.</param>
		/// <returns>The policy service level.</returns>
		public PolicyCounter ReportFailure(string serviceName, long duration, DateTimeOffset timestamp)
		{
			services.AddOrUpdate(serviceName, new ServiceCounter().ReportSuccess(duration, timestamp), (k, s) => s.ReportSuccess(duration, timestamp));

			return this;
		}

		/// <summary>
		/// The services.
		/// </summary>
		public IReadOnlyDictionary<string, ServiceCounter> Services => new ReadOnlyDictionary<string, ServiceCounter>(services);
	}
}
