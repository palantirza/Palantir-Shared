namespace Palantir.Features
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	/// <summary>
	/// Extension methods for the <see cref="IFeatureConfiguration{TFeatures}"/> interface.
	/// </summary>
	public static class FeatureConfigurationExtensions
    {
		/// <summary>
		/// Fetches the feature rules.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <param name="featureSelector">the feature selector.</param>
		/// <typeparam name="TFeatures">The features type.</typeparam>
		/// <returns>The feature rules.</returns>
		public static async Task<IEnumerable<IFeatureRule>> AsyncGetFeatureRules<TFeatures>(this IFeatureConfiguration<TFeatures> configuration, Expression<Func<TFeatures, bool>> featureSelector) where TFeatures: class, new()
		{
			var name = ExpressionParser.GetQualifiedName(featureSelector);
			return await configuration.AsyncGetFeatureRules(name);
		}

		/// <summary>
		/// Fetches the feature rules.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <param name="featureSelector">the feature selector.</param>
		/// <typeparam name="TFeatures">The features type.</typeparam>
		/// <returns>The feature rules.</returns>
		public static IEnumerable<IFeatureRule> GetFeatureRules<TFeatures>(this IFeatureConfiguration<TFeatures> configuration, Expression<Func<TFeatures, bool>> featureSelector) where TFeatures : class, new()
		{
			return AsyncGetFeatureRules(configuration, featureSelector).Result;
		}

		/// <summary>
		/// Fetches the feature rules.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <param name="featureSelector">the feature selector.</param>
		/// <typeparam name="TFeatures">The features type.</typeparam>
		/// <returns>The feature rules.</returns>
		public static IEnumerable<IFeatureRule> GetFeatureRules<TFeatures>(this IFeatureConfiguration<TFeatures> configuration, string feature) where TFeatures : class, new()
		{
			return configuration.AsyncGetFeatureRules(feature).Result;
		}
	}
}
