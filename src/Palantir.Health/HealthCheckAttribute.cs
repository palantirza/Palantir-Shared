namespace Palantir.Health
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Indicates the health checks to be performed for an action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class HealthCheckAttribute : Attribute, IFilterFactory
    {
		/// <summary>
		/// Initializes the HealthCheck policy.
		/// </summary>
		/// <param name="policyName">The policy name.</param>
		public HealthCheckAttribute(string policyName)
		{
			Contract.Requires(!string.IsNullOrEmpty(policyName));

			PolicyName = policyName;
		}

        /// <summary>
        /// Indicates if the attribute is reusable.
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The polcy name for the health check to perform.
        /// </summary>
        public string PolicyName { get; }

		/// <summary>
		/// Creates a filter for the attribute.
		/// </summary>
		/// <param name="serviceProvider">The service provider.</param>
		/// <returns>The filter metadata.</returns>
		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			var service = serviceProvider.GetRequiredService<IHealthMonitoringService>();
			var environment = serviceProvider.GetRequiredService<IHostingEnvironment>();

			return new HealthCheckFilter(service, environment);
		}
	}
}
