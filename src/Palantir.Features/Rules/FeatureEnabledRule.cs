namespace Palantir.Features.Rules
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// The simplest rule, the feature is enabled or not.
	/// </summary>
	[FeatureRuleTypeName("enabled")]
	public sealed class FeatureEnabledRule : IFeatureRule
	{
		public static readonly FeatureEnabledRule FeatureEnabled = new FeatureEnabledRule(true);
		public static readonly FeatureEnabledRule FeatureDisabled = new FeatureEnabledRule(false);

		/// <summary>
		/// Initializes a new instance of the <see cref="FeatureEnabledRule"/> class.
		/// </summary>
		/// <param name="enabled">The enabled state.</param>
		public FeatureEnabledRule(bool enabled)
		{
			Enabled = enabled;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FeatureEnabledRule"/> class.
		/// </summary>
		/// <param name="config">The configuration.</param>
		public FeatureEnabledRule(Configuration.RuleOptions config)
		{
			Enabled = config.Enabled ?? false;
		}

		/// <summary>
		/// The enabled state for the rule.
		/// </summary>
		public bool Enabled { get; }

		/// <summary>
		/// Evaluates the rule.
		/// </summary>
		/// <param name="context">The feature context.</param>
		/// <returns>The evaluation result.</returns>
		public bool EvaluateRule(IFeatureContext context)
		{
			return Enabled;
		}
	}
}
