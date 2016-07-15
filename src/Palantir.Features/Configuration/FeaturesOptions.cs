namespace Palantir.Features.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;

	/// <summary>
	/// Contains the configuration for a set of features.
	/// </summary>
	public sealed class FeaturesOptions<TFeatures> where TFeatures: class
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FeaturesOptions"/> class.
		/// </summary>
		public FeaturesOptions()
		{
			Features = new Collection<FeatureOptions>();
		}

		/// <summary>
		/// The features.
		/// </summary>
		public Collection<FeatureOptions> Features { get; set; }
	}
}