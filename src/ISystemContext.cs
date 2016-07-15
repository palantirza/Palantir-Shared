namespace Palantir
{
	using System;
	using System.Security.Principal;

	public interface ISystemContext
    {
		IPrincipal CurrentUser { get; }

		DateTimeOffset UtcNow { get; }
    }
}
