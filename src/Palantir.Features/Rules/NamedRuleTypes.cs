namespace Palantir.Features.Rules
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Threading.Tasks;

	/// <summary>
	/// Collects the named rule types.
	/// </summary>
	internal static class NamedRuleTypes
    {
		private static readonly Dictionary<string, Type> namedRuleTypes;

		/// <summary>
		/// Initializes the <see cref="NamedRuleTypes"/> type.
		/// </summary>
		static NamedRuleTypes()
		{
			var featureRuleType = typeof(IFeatureRule);
			namedRuleTypes = (from t in typeof(FeatureEnabledRule).Assembly.GetTypes()
							  where featureRuleType.IsAssignableFrom(t)
							  let a = t.GetCustomAttribute<FeatureRuleTypeNameAttribute>()
							  where a != null
							  select new { Type = t, Name = a.Name }).ToDictionary(x => x.Name, x => x.Type);
		}

		/// <summary>
		/// Fetches the named rule type.
		/// </summary>
		/// <param name="name">The rule type name.</param>
		/// <returns>The rule type, or null.</returns>
		public static Type GetNamedRuleType(string name)
		{
			if (namedRuleTypes.ContainsKey(name))
				return namedRuleTypes[name];

			return null;
		}
    }
}
