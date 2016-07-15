namespace Palantir.Mvc.Authentication
{
    using Microsoft.AspNetCore.Builder;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class AuthenticationExtensions
    {
		public static void UseTokenAuthentication(this IApplicationBuilder builder)
		{
			builder.UseMiddleware<TokenAuthentication>();
		}
	}
}
