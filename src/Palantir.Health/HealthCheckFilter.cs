namespace Palantir.Health
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// The filter which performs health checks.
    /// </summary>
    public class HealthCheckFilter : IAsyncResultFilter
	{
		private IHealthMonitoringService service;
		private bool isProduction;

		/// <summary>
		/// Initializes a new instance of the <see cref="HealthCheckFilter"/>.
		/// </summary>
		/// <param name="service">The health monitoring service.</param>
		/// <param name="environment">The hosting environment.</param>
		public HealthCheckFilter(IHealthMonitoringService service, IHostingEnvironment environment)
		{
			Contract.Requires(service != null);

			this.service = service;
			isProduction = environment.IsProduction();
		}
		
		/// <summary>
		/// Executes on a result execution.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="next">The next execution delegate.</param>
		/// <returns>The task.</returns>
		public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			var controllerType = context.Controller.GetType();
			var controllerCheck = controllerType.GetCustomAttribute<HealthCheckAttribute>();
			var actionCheck = controllerType.GetMethod(((ControllerActionDescriptor)context.ActionDescriptor).ActionName, context.ActionDescriptor.Parameters.Select(x => x.ParameterType).ToArray()).GetCustomAttribute<HealthCheckAttribute>();

			var check = actionCheck ?? controllerCheck;

			Task<HealthReport> checkResult = null;
			if (check != null)
				checkResult = service.CheckHealthAsync(check.PolicyName);
			if (checkResult != null)
			{
				var result = checkResult.Result;

				string resultText = result.Status.ToString();

				// We only provide policy name and indicator details if the machine is not in Production,
				// for security reasons
				if (!isProduction)
				{
					resultText = "[" + check.PolicyName + "]: " + resultText;

					if (result.Status != HealthStatus.OK)
						resultText += " - " + string.Join(", ", result.Indicators.Where(x => x.Status == result.Status).Select(x => x.StatusText));
				}

				context.HttpContext.Response.Headers.Add("X-Health-Check", resultText);
			}

			await next();

		}
	}
}
