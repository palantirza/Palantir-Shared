#if DNX46
namespace Palantir.Mvc
{
	using Microsoft.AspNet.Builder;
	using Microsoft.AspNet.Http;
	using Serilog;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// The middleware for tracking correlation ID's.
	/// </summary>
	public sealed class CorrelationMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;

		private static readonly string CORRELATION_ID_HTTP_HEADER = Headers.CorrelationId;

		public CorrelationMiddleware(RequestDelegate next, ILogger logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			string correlationId = null;

			var headers = context.Request.Headers;
			
			correlationId = headers.GetCommaSeparatedValues(CORRELATION_ID_HTTP_HEADER).FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));
			
			if (string.IsNullOrWhiteSpace(correlationId))
				correlationId = Guid.NewGuid().ToString();

			ActivityScope scope = new ActivityScope(null, correlationId);

			await _next(context);

			context.Response.Headers.Add(CORRELATION_ID_HTTP_HEADER, scope.Id);
			scope.Dispose();
		}
	}
}
#endif
