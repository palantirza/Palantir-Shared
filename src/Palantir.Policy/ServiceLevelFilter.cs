namespace Palantir.Policy
{
    using Microsoft.Owin.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Threading.Tasks;
    using Palantir.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Controllers;

    /// <summary>
    /// The filter which performs health checks.
    /// </summary>
    public class ServiceLevelFilter : IAsyncActionFilter, IAsyncResultFilter, IAsyncExceptionFilter
	{
		private IServiceLevelMonitoringService service;
		private ISystemClock systemClock;
		private DateTimeOffset timestamp;
		private string policyName;
		private string serviceName;
		private bool isProduction;
		private Stopwatch stopwatch;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceLevelFilter"/>.
		/// </summary>
		/// <param name="service">The service level monitoring service.</param>
		/// <param name="environment">The hosting environment.</param>
		/// <param name="systemClock">The system clock.</param>
		public ServiceLevelFilter(IServiceLevelMonitoringService service, IHostingEnvironment environment, ISystemClock systemClock)
		{
			Contract.Requires(service != null);

			this.service = service;
			this.systemClock = systemClock;
			isProduction = environment.IsProduction();
		}
		
		/// <summary>
		/// Fired when the action executes asynchronously.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="next">The next action.</param>
		/// <returns>The task.</returns>
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			timestamp = systemClock.UtcNow;
			stopwatch = Stopwatch.StartNew();

			var controllerType = context.Controller.GetType();
			var controllerCheck = controllerType.GetCustomAttribute<ServiceLevelAttribute>();
			var actionCheck = controllerType.GetMethod(((ControllerActionDescriptor)context.ActionDescriptor).ActionName, context.ActionDescriptor.Parameters.Select(x => x.ParameterType).ToArray()).GetCustomAttribute<ServiceLevelAttribute>();

			var check = actionCheck ?? controllerCheck;
			
			policyName = check.PolicyName;
			serviceName = '/' + context.ActionDescriptor.AttributeRouteInfo.Template;

			await next();
		}

		/// <summary>
		/// Fired when the result executes asynchronously.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="next">The next action.</param>
		/// <returns>The task.</returns>
		public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			stopwatch.Stop();
			if (context.HttpContext.Response.StatusCode > 500)
#if DNX46
				service.ReportFailure(policyName, serviceName, ActivityScope.Current?.Id, stopwatch.ElapsedMilliseconds, timestamp);
#else
				service.ReportFailure(policyName, serviceName, null, stopwatch.ElapsedMilliseconds, timestamp);
#endif
			else
#if DNX46
				service.ReportSuccess(policyName, serviceName, ActivityScope.Current?.Id, stopwatch.ElapsedMilliseconds, timestamp);
#else
				service.ReportSuccess(policyName, serviceName, null, stopwatch.ElapsedMilliseconds, timestamp);
#endif

			var report = await service.GetServiceReport(policyName, serviceName);
			var resultText = report.Achieved ? "Achieved" : "Breached";
			if (!isProduction)
			{
				resultText = "[" + policyName + "]: " + resultText;

				resultText += " - 95%: " + report.Detail.Duration.Percentiles[95] + ", 99%: " + report.Detail.Duration.Percentiles[99] + ", Mean: " + report.Detail.Duration.Mean.ToString("0.00") + ", Max: " + report.Detail.Duration.Maximum;
			}
			context.HttpContext.Response.Headers.Add("X-Service-Level", resultText);

			await next();
		}

		/// <summary>
		/// Fired when an exception occurs.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>The task result.</returns>
		public Task OnExceptionAsync(ExceptionContext context)
		{
			stopwatch.Stop();
#if DNX46
			service.ReportFailure(policyName, serviceName, ActivityScope.Current?.Id, stopwatch.ElapsedMilliseconds, timestamp);
#else
			service.ReportFailure(policyName, serviceName, null, stopwatch.ElapsedMilliseconds, timestamp);
#endif

			return Task.FromResult(0);
		}
	}
}
