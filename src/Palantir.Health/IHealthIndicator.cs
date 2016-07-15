namespace Palantir.Health
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Represents a health indicator.
	/// </summary>
    public interface IHealthIndicator
    {
		/// <summary>
		/// Peforms a health check.
		/// </summary>
		/// <returns>The health.</returns>
		Task<Health> CheckHealthAsync();
    }
}
