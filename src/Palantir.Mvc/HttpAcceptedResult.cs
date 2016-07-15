namespace Palantir.Mvc
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    public sealed class HttpAcceptedResult : StatusCodeResult
    {
		public HttpAcceptedResult()
			: base((int)HttpStatusCode.Accepted)
		{
		}
	}
}
