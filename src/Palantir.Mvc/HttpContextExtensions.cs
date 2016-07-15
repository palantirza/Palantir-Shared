namespace Palantir.Mvc
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using System;

    /// <summary>
    /// Provides extensions for <see cref="HttpContext"/>
    /// </summary>
    public static class HttpContextExtensions
	{
		/// <summary>
		/// Gets the client ip address.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException"></exception>
		public static string GetClientIPAddress(this HttpContext context)
		{
			if (null == context)
			{
				throw new ArgumentNullException(nameof(context));
			}

			var connection = context.Features.Get<IHttpConnectionFeature>();
			return connection?.RemoteIpAddress?.ToString();
		}
	}
}
