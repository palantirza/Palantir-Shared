namespace Palantir.Mvc
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    public sealed class HttpConflictResult : StatusCodeResult
    {
		public HttpConflictResult()
			: base((int)HttpStatusCode.Conflict)
		{
		}
	}
}
