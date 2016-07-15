namespace Palantir.Policy
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Owin.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Service levels are applied to actions, and define the reliability and performance characteristics that are required for
    /// the service.
    /// 
    /// Reliability guarantees, performance guarantees, scalability guarantees can all be applied strictly per call, or
    /// statistically over a sample set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class ServiceLevelAttribute : Attribute, IFilterFactory
	{
		/// <summary>
		/// Specifies the Service Level policy for an Action.
		/// </summary>
		/// <param name="policyName">The policy name.</param>
		public ServiceLevelAttribute(string policyName)
		{
			Contract.Requires(!string.IsNullOrEmpty(policyName));

			PolicyName = policyName;
		}

        /// <summary>
        /// Gets a value that indicates if the result of Microsoft.AspNetCore.Mvc.Filters.IFilterFactory.CreateInstance(System.IServiceProvider)
        //     can be reused across requests.
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
			var service = serviceProvider.GetRequiredService<IServiceLevelMonitoringService>();
			var environment = serviceProvider.GetRequiredService<IHostingEnvironment>();
			var systemClock = serviceProvider.GetRequiredService<ISystemClock>();

			return new ServiceLevelFilter(service, environment, systemClock);
		}
	}
}
