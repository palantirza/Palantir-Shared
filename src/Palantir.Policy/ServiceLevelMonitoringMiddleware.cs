namespace Palantir.Policy
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// The service level middleware.
    /// </summary>
    public sealed class ServiceLevelMonitoringMiddleware
	{
		private readonly RequestDelegate next;
		private readonly IServiceLevelMonitoringService serviceLevelMonitoringService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceLevelMonitoringMiddleware"/> class.
		/// </summary>
		/// <param name="next">The next request handler.</param>
		/// <param name="serviceLevelMonitoringService">The service level monitoring service.</param>
		/// <param name="loggerFactory">The logger factory.</param>
		public ServiceLevelMonitoringMiddleware(RequestDelegate next, IServiceLevelMonitoringService serviceLevelMonitoringService, ILoggerFactory loggerFactory)
		{
			Contract.Requires(next != null);
			Contract.Requires(serviceLevelMonitoringService != null);
			Contract.Requires(loggerFactory != null);

			this.serviceLevelMonitoringService = serviceLevelMonitoringService;
			Logger = loggerFactory.CreateLogger(this.GetType().FullName);

			this.next = next;
		}

		/// <summary>
		/// Handles the request.
		/// </summary>
		/// <param name="context">The HTTP context.</param>
		/// <returns>The task,</returns>
		public async Task Invoke(HttpContext context)
		{
			if (context.Request.Path == "/info/sla")
			{
				var report = await serviceLevelMonitoringService.GetServiceLevelReport();

				var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings();
				serializerSettings.Converters.Add(new StringEnumConverter());
				var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault(serializerSettings);
				using (var writer = new StreamWriter(context.Response.Body))
				{
					serializer.Serialize(writer, report);
				}

				context.Response.StatusCode = (int)HttpStatusCode.OK;
			}
			await next(context);
		}
		
		/// <summary>
		/// The logger.
		/// </summary>
		public ILogger Logger { get; set; }
	}
}
