namespace Palantir.Features
{
	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	/// <summary>
	/// Allows using code to determine if features are enabled or not.
	/// </summary>
	/// <typeparam name="TFeatures">The feature settings.</typeparam>
	public interface IFeatureRouter<TFeatures> where TFeatures: class, new()
    {
		/// <summary>
		/// Fetches the feature decisions.
		/// </summary>
		/// <param name="context">The feature context to make the decisions in.</param>
		/// <returns>The feature decisions.</returns>
		Task<TFeatures> AsyncGetFeatureDecisions(IFeatureContext context = null);

		/// <summary>
		/// Indicates whether the feature is enabled or not.
		/// </summary>
		/// <param name="feature">The feature.</param>
		/// <param name="context">The feature context, if any.</param>
		/// <returns>true if the feature is enabled, false otherwise.</returns>
		Task<bool> AsyncIsFeatureEnabled(string feature, IFeatureContext context = null);
    }
}
