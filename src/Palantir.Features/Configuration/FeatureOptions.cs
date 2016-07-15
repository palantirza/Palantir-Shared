namespace Palantir.Features.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Configuration value for a feature.
	/// </summary>
	public sealed class FeatureOptions
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="FeatureOptions"/> class.
		/// </summary>
		public FeatureOptions()
		{
			Rules = new Collection<RuleOptions>();
		}

		/// <summary>
		/// The feature name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The feature rules.
		/// </summary>
		public Collection<RuleOptions> Rules { get; set; }
    }
}
