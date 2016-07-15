namespace Palantir.Mvc
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Owin.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;

    public class DefaultSystemContext : ISystemContext, ISystemClock
	{
		private IHttpContextAccessor contextAccessor;

		public DefaultSystemContext(IHttpContextAccessor contextAccessor)
		{
			this.contextAccessor = contextAccessor;
		}

		public IPrincipal CurrentUser
		{
			get
			{
				return contextAccessor.HttpContext.User;
			}
		}

		public DateTimeOffset UtcNow
		{
			get
			{
				return DateTimeOffset.UtcNow;
			}
		}
	}
}
