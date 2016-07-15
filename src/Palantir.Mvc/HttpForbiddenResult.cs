namespace Palantir.Mvc
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net;

    /// <summary>
    /// A result for 403 - Forbidden.
    /// </summary>
    public sealed class HttpForbiddenResult : StatusCodeResult
	{
		public HttpForbiddenResult()
			: base((int)HttpStatusCode.Forbidden)
		{
		}
	}
}
