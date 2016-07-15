namespace Palantir.Features
{
	using Configuration;
	using Serilog;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Threading.Tasks;

	/// <summary>
	/// Allows using code to determine if features are enabled or not.
	/// </summary>
	/// <typeparam name="TFeatures">The feature settings.</typeparam>
	public sealed class FeatureRouter<TFeatures> : IFeatureRouter<TFeatures> where TFeatures : class, new()
	{
		private readonly IFeatureConfiguration<TFeatures> configuration;
		private readonly ILogger log;
		private readonly Type featureType = typeof(TFeatures);
		private readonly Lazy<Dictionary<string, PropertyInfo>> featureProperties;

		/// <summary>
		/// Initializes a new instance of the <see cref="FeatureRouter{TFeatures}"/> class.
		/// </summary>
		/// <param name="configuration">The feature configuration.</param>
		/// <param name="log">The logging.</param>
		public FeatureRouter(IFeatureConfiguration<TFeatures> configuration, ILogger log)
		{
			Contract.Requires(configuration != null);
			Contract.Requires(log != null);

			this.configuration = configuration;
			this.log = log;

			featureProperties = new Lazy<Dictionary<string, PropertyInfo>>(() =>
			{
				return (from p in featureType.GetProperties()
						where p.CanRead && p.GetGetMethod().IsPublic
						let n = p.GetCustomAttribute<FeatureNameAttribute>()
						select new { Name = n == null ? p.Name : n.Name, Property = p })
						.ToDictionary(x => x.Name, x => x.Property);
			});
		}

		/// <summary>
		/// Fetches the feature decisions.
		/// </summary>
		/// <param name="context">The feature context.</param>
		/// <returns>The feature decisions.</returns>
		public async Task<TFeatures> AsyncGetFeatureDecisions(IFeatureContext context = null)
		{
			log.Verbose("Beginning decision evaluation for {FeatureClass}", featureType.Name);

			var decision = new TFeatures();
			foreach (var feature in featureProperties.Value)
			{
				if (!feature.Value.CanWrite)
					throw new ArgumentException(SR.Err_FeaturePropertyMustBeWritable(feature.Key));

				var name = feature.Key;

				var featureEnabled = await AsyncIsFeatureEnabled(name, context);
				feature.Value.SetValue(decision, featureEnabled);
			}

			log.Verbose("Ending decision evaluation for {FeatureClass}, evaluated {@FeatureDecisions}", featureType.Name, decision);

			return decision;
		}

		/// <summary>
		/// Indicates whether the feature is enabled or not.
		/// </summary>
		/// <param name="feature">The feature.</param>
		/// <param name="context">The feature context, if any.</param>
		/// <returns>true if the feature is enabled, false otherwise.</returns>
		public async Task<bool> AsyncIsFeatureEnabled(string feature, IFeatureContext context = null)
		{
			log.Verbose("Beginning rule evaluation of feature {Feature}", feature);
			var rules = await configuration.AsyncGetFeatureRules(feature);
			var enabled = false;
			foreach (var rule in rules)
			{
				enabled = rule.EvaluateRule(context);

				log.Verbose("Feature {Feature} rule {Rule} evaluated {FeatureEnabled}", feature, rule.GetType().Name, enabled);
				if (!enabled)
					break;
			}

			log.Verbose("Ending rule evaluation of feature {Feature}, evaluated {FeatureEnabled}", feature, enabled);

			return enabled;
		}
	}
}
