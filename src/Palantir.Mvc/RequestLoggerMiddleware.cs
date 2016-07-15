namespace Palantir.Mvc
{
    using System.Threading.Tasks;
    using Serilog;
    using System.Diagnostics;
    using Microsoft.Owin.Infrastructure;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Implementation of <see cref="RequestLoggerMiddleware"/>
    /// </summary>
    public class RequestLoggerMiddleware
	{
		/// <summary>
		/// The next middleware to execute
		/// </summary>
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;
		private readonly ISystemClock _clock;

		public RequestLoggerMiddleware(RequestDelegate next, ILogger logger, ISystemClock clock)
		{
			_next = next;
			_logger = logger;
			_clock = clock;
		}

		public async Task Invoke(HttpContext context)
		{
			var start = _clock.UtcNow;
			var watch = Stopwatch.StartNew();
			await _next(context);
			watch.Stop();
			if (_logger.IsEnabled(Serilog.Events.LogEventLevel.Debug))
			{
				var logTemplate = @"
                                Client IP: {clientIP}
                                Request path: {requestPath}
                                Request content type: {requestContentType}
                                Request content length: {requestContentLength}
                                Response content type: {responseContentType}
                                Response content length: {responseContentLength},
                                Response status code: {statusCode}
                                Start time: {startTime}
                                Duration: {duration} ms";

				_logger.Information(logTemplate,
					context.GetClientIPAddress(),
					context.Request.Path,
					context.Request.ContentType,
					context.Request.ContentLength,
					context.Response.ContentType,
					context.Response.ContentLength,
					context.Response.StatusCode,
					start,
					watch.ElapsedMilliseconds);
			}
		}
	}
}
