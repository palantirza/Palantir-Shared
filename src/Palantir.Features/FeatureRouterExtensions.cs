namespace Palantir.Features
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	/// <summary>
	/// Extension methods for feature routers.
	/// </summary>
	public static class FeatureRouterExtensions
	{
		/// <summary>
		/// Fetches the feature decisions.
		/// </summary>
		/// <param name="router">The feature router.</param>
		/// <param name="context">The feature context, if any.</param>
		/// <typeparam name="TFeatures">The features type.</typeparam>
		/// <returns>The feature decisions.</returns>
		public static TFeatures GetFeatureDecisions<TFeatures>(this IFeatureRouter<TFeatures> router, IFeatureContext context = null) where TFeatures : class, new()
		{
			return router.AsyncGetFeatureDecisions(context).Result;
		}
		
		/// <summary>
		/// Indicates whether the feature is enabled or not.
		/// </summary>
		/// <param name="router">The feature router.</param>
		/// <param name="featureSelector">The feature selector.</param>
		/// <param name="context">The feature context, if any.</param>
		/// <typeparam name="TFeatures">The features type.</typeparam>
		/// <returns>true if the feature is enabled, false otherwise.</returns>
		public static async Task<bool> AsyncIsFeatureEnabled<TFeatures>(this IFeatureRouter<TFeatures> router, Expression<Func<TFeatures, bool>> featureSelector, IFeatureContext context = null) where TFeatures : class, new()
		{
			var name = ExpressionParser.GetQualifiedName(featureSelector);
			return await router.AsyncIsFeatureEnabled(name, context);
		}

		/// <summary>
		/// Indicates whether the feature is enabled or not.
		/// </summary>
		/// <param name="router">The feature router.</param>
		/// <param name="featureSelector">The feature selector.</param>
		/// <param name="context">The feature context, if any.</param>
		/// <typeparam name="TFeatures">The features type.</typeparam>
		/// <returns>true if the feature is enabled, false otherwise.</returns>
		public static bool IsFeatureEnabled<TFeatures>(this IFeatureRouter<TFeatures> router, Expression<Func<TFeatures, bool>> featureSelector, IFeatureContext context = null) where TFeatures : class, new()
		{
			return AsyncIsFeatureEnabled(router, featureSelector, context).Result;
		}

		/// <summary>
		/// Indicates whether the feature is enabled or not.
		/// </summary>
		/// <param name="router">The feature router.</param>
		/// <param name="feature">The feature name.</param>
		/// <param name="context">The feature context, if any.</param>
		/// <typeparam name="TFeatures">The features type.</typeparam>
		/// <returns>true if the feature is enabled, false otherwise.</returns>
		public static bool IsFeatureEnabled<TFeatures>(this IFeatureRouter<TFeatures> router, string feature, IFeatureContext context = null) where TFeatures : class, new()
		{
			return router.AsyncIsFeatureEnabled(feature, context).Result;
		}

		/// <summary>
		/// Indicates whether the feature is enabled or not.
		/// </summary>
		/// <param name="router">The feature router.</param>
		/// <param name="featureSelector">The feature selector.</param>
		/// <param name="contextProperties">The feature context, if any.</param>
		/// <typeparam name="TFeatures">The features type.</typeparam>
		/// <returns>true if the feature is enabled, false otherwise.</returns>
		public static async Task<bool> AsyncIsFeatureEnabled<TFeatures>(this IFeatureRouter<TFeatures> router, Expression<Func<TFeatures, bool>> featureSelector, object contextProperties) where TFeatures : class, new()
		{
			var context = new FeatureContext(contextProperties);

			var name = ExpressionParser.GetQualifiedName(featureSelector);
			return await router.AsyncIsFeatureEnabled(name, context);
		}

		/// <summary>
		/// Indicates whether the feature is enabled or not.
		/// </summary>
		/// <param name="router">The feature router.</param>
		/// <param name="featureSelector">The feature selector.</param>
		/// <param name="contextProperties">The feature context properties, if any.</param>
		/// <typeparam name="TFeatures">The features type.</typeparam>
		/// <returns>true if the feature is enabled, false otherwise.</returns>
		public static bool IsFeatureEnabled<TFeatures>(this IFeatureRouter<TFeatures> router, Expression<Func<TFeatures, bool>> featureSelector, object contextProperties) where TFeatures : class, new()
		{
			return AsyncIsFeatureEnabled(router, featureSelector, contextProperties).Result;
		}

		/// <summary>
		/// Indicates whether the feature is enabled or not.
		/// </summary>
		/// <param name="router">The feature router.</param>
		/// <param name="feature">The feature name.</param>
		/// <param name="contextProperties">The feature context properties, if any.</param>
		/// <typeparam name="TFeatures">The features type.</typeparam>
		/// <returns>true if the feature is enabled, false otherwise.</returns>
		public static bool IsFeatureEnabled<TFeatures>(this IFeatureRouter<TFeatures> router, string feature, object contextProperties) where TFeatures : class, new()
		{
			var context = new FeatureContext(contextProperties);

			return router.AsyncIsFeatureEnabled(feature, context).Result;
		}
	}
}
