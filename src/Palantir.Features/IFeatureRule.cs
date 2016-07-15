namespace Palantir.Features
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Represents a rule that forms part of a feature.
	/// </summary>
	public interface IFeatureRule
    {
		/// <summary>
		/// Evaluates the rule.
		/// </summary>
		/// <param name="context">The feature context to evaluate the rule within.</param>
		/// <returns>true if the feature is enabled, false otherwise.</returns>
		bool EvaluateRule(IFeatureContext context);
    }
}
