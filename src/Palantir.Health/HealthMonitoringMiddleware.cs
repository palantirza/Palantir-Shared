namespace Palantir.Health
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// The health monitoring middleware.
    /// </summary>
    public sealed class HealthMonitoringMiddleware
	{
		private readonly RequestDelegate next;
		private readonly IHealthMonitoringService healthMonitoringService;

		/// <summary>
		/// Initializes a new instance of the <see cref="HealthMonitoringMiddleware"/> class.
		/// </summary>
		/// <param name="next">The next request handler.</param>
		/// <param name="options">The health monitoring options.</param>
		/// <param name="loggerFactory">The logger factory.</param>
		public HealthMonitoringMiddleware(RequestDelegate next, IHealthMonitoringService healthMonitoringService, ILoggerFactory loggerFactory)
		{
			Contract.Requires(next != null);
			Contract.Requires(healthMonitoringService != null);
			Contract.Requires(loggerFactory != null);

			this.healthMonitoringService = healthMonitoringService;
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
			if (context.Request.Path == "/info/health")
			{
				var report = await healthMonitoringService.CheckAllHealthAsync();

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
		/// The health monitoring options.
		/// </summary>
		public HealthMonitoringOptions Options { get; }

		/// <summary>
		/// The logger.
		/// </summary>
		public ILogger Logger { get; set; }
	}
}
