namespace Palantir.Mvc.Authentication
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public sealed class TokenAuthentication
    {
		private readonly RequestDelegate next;

		public TokenAuthentication(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			// TODO: Implement, for now we just create an identity

			context.User = new ClaimsPrincipal(new ClaimsIdentity("Custom"));

			await next(context);
		}
	}
}
