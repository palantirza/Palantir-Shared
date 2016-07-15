namespace Palantir.Features
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	/// <summary>
	/// Provides the configuration for a feature rule.
	/// </summary>
	/// <typeparam name="TFeatures">The features class.</typeparam>
	public interface IFeatureConfiguration<TFeatures> where TFeatures : class, new()
	{
		/// <summary>
		/// Fetches the feature rules.
		/// </summary>
		/// <param name="feature">The feature.</param>
		/// <returns>The feature rules.</returns>
		Task<IEnumerable<IFeatureRule>> AsyncGetFeatureRules(string feature);
	}
}
