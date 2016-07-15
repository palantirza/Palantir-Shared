namespace Palantir.Features.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// The rule configuration.
	/// </summary>
    public sealed class RuleOptions
    {
		/// <summary>
		/// The rule type.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// The enabled property state.
		/// </summary>
		public bool? Enabled { get; set; }
    }
}
