namespace Palantir.Features
{
	using Configuration;
	using Microsoft.Extensions.Options;
	using Rules;
	using Serilog;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Threading.Tasks;

	/// <summary>
	/// A feature configuration based on the .NET options model.
	/// </summary>
	/// <typeparam name="TFeatures">The features type.</typeparam>
	public class OptionsModelFeatureConfiguration<TFeatures> : IFeatureConfiguration<TFeatures> where TFeatures : class, new()
	{
		private readonly MultiValueDictionary<string, IFeatureRule> rules = new MultiValueDictionary<string, IFeatureRule>();
		private static readonly IEnumerable<IFeatureRule> EmptyFeatures = new IFeatureRule[] { };
		private readonly ILogger log;
		private readonly Dictionary<string, ObsoleteAttribute> obsoleteFeatures = new Dictionary<string, ObsoleteAttribute>();

		/// <summary>
		/// Initializes a new instance of the feature configuration.
		/// </summary>
		/// <param name="configuration">The configuration settings.</param>
		/// <param name="logger">The logger.</param>
		public OptionsModelFeatureConfiguration(IOptions<FeaturesOptions<TFeatures>> configuration, ILogger logger)
		{
			Contract.Requires(configuration != null);

			log = logger;
			
			foreach (var featureConfig in configuration.Value.Features)
			{
				foreach (var ruleConfig in featureConfig.Rules)
				{
					var ruleType = Type.GetType(ruleConfig.Type, false, true) ?? NamedRuleTypes.GetNamedRuleType(ruleConfig.Type);
					if (ruleType == null)
						throw new ArgumentException(SR.Err_NotARuleType(ruleConfig.Type));

					var rule = Activator.CreateInstance(ruleType, ruleConfig) as IFeatureRule;

					if (rule == null)
						throw new ArgumentException(SR.Err_NotAFeatureRuleType(ruleType));

					rules.Add(featureConfig.Name, rule);
				}
			}

			// Store the obsolete attributes.
			var featureType = typeof(TFeatures);
			foreach (var feature in featureType.GetProperties())
			{
				var obsoleteAttribute = feature.GetCustomAttribute<ObsoleteAttribute>();
				if (obsoleteAttribute != null)
				{
					var nameAttribute = feature.GetCustomAttribute<FeatureNameAttribute>();
					var name = nameAttribute == null ? featureType.Name + "." + feature.Name : nameAttribute.Name;

					obsoleteFeatures.Add(name, obsoleteAttribute);

					if (obsoleteAttribute.IsError)
						log.Information("Feature {Feature} is marked as deprecated", name);
					else
						log.Information("Feature {Feature} is marked as obsolete", name);
				}
			}
		}
		
		/// <summary>
		/// Fetches the feature rules.
		/// </summary>
		/// <param name="feature">The feature.</param>
		/// <returns>The feature rules.</returns>
		public Task<IEnumerable<IFeatureRule>> AsyncGetFeatureRules(string feature)
		{
			if (obsoleteFeatures.ContainsKey(feature))
			{
				var obsolete = obsoleteFeatures[feature];
				if (obsolete.IsError)
				{
					log.Error("Deprecated Feature: {Feature}", feature);
					throw new ObsoleteFeatureException(SR.Err_ObsoleteFeatureError(feature));
				}
				else
					log.Warning("Obsolete Feature: {Feature}", feature);
			}

			if (!rules.ContainsKey(feature))
				return Task.FromResult(EmptyFeatures);

			IEnumerable<IFeatureRule> result = rules[feature].AsEnumerable();
			return Task.FromResult(result);
		}
	}
}
