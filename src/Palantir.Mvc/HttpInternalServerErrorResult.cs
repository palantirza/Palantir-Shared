namespace Palantir.Mvc
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class HttpInternalServerErrorResult : StatusCodeResult
    {
		public HttpInternalServerErrorResult()
			: base((int)System.Net.HttpStatusCode.InternalServerError)
		{
		}
    }
}
