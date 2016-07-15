namespace Palantir.Health
{
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Extension methods for health monitoring.
	/// </summary>
	public static class HealthMonitoringServiceCollectionExtensions
    {
		/// <summary>
		/// Adds health monitoring services to the specified <see cref="IServiceCollection" />. 
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
		/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
		public static IServiceCollection AddHealthMonitoring(this IServiceCollection services)
		{
			Contract.Requires(services != null);

			services.TryAdd(ServiceDescriptor.Transient<IHealthMonitoringService, DefaultHealthMonitoringService>());
			services.TryAdd(ServiceDescriptor.Transient<IHealthMonitoringPolicyProvider, DefaultHealthMonitoringPolicyProvider>());
			services.AddMemoryCache();

			return services;
		}

	}
}
