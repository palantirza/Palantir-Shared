namespace Palantir.Health
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// A policy builder for health monitoring.
	/// </summary>
    public sealed class HealthMonitoringPolicyBuilder
    {
		private readonly List<IHealthIndicator> healthIndicators = new List<IHealthIndicator>();

		/// <summary>
		/// Adds monitoring for disk space.
		/// </summary>
		/// <param name="drive">The drive to monitor.</param>
		/// <param name="warningThreshold">The threshold to warn at (in bytes).</param>
		/// <param name="errorThreshold">The threshold to warn at (in bytes).</param>
		/// <returns>The policy builder.</returns>
		public HealthMonitoringPolicyBuilder MonitorDiskSpace(string drive = DiskSpaceHealthIndicator.SystemDrive, long warningThreshold = DiskSpaceHealthIndicator.DefaultWarningThreshold, long errorThreshold = DiskSpaceHealthIndicator.DefaultErrorThreshold)
		{
			var indicator = new DiskSpaceHealthIndicator(drive, warningThreshold, errorThreshold);

			healthIndicators.Add(indicator);

			return this;
		}

		/// <summary>
		/// Builds the health monitoring policy.
		/// </summary>
		/// <returns>The built policy.</returns>
		public HealthMonitoringPolicy Build()
		{
			return new HealthMonitoringPolicy(healthIndicators);
		}
    }
}