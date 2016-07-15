namespace Palantir.Features
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// A collection of feature rules.
	/// </summary>
	public class FeatureRules : IEnumerable<IFeatureRule>
	{
		private List<IFeatureRule> rules = new List<IFeatureRule>();

		/// <summary>
		/// Initializes a new instance of the <see cref="FeatureRules"/> class.
		/// </summary>
		/// <param name="rules">The rules to initialize the collection with.</param>
		public FeatureRules(IEnumerable<IFeatureRule> rules = null)
		{
			if (rules != null)
				AddRange(rules);
		}

		/// <summary>
		/// Adds a set of feature rules.
		/// </summary>
		/// <param name="rules">The rules to add.</param>
		public void AddRange(IEnumerable<IFeatureRule> rules)
		{
			Contract.Requires(rules != null);

			this.rules.AddRange(rules);
		}

		/// <summary>
		/// Adds a rule to the list of feature rules.
		/// </summary>
		/// <param name="rule">The rule.</param>
		public void Add(IFeatureRule rule)
		{
			Contract.Requires(rule != null);

			rules.Add(rule);
		}

		public int Count => rules.Count;

		/// <summary>
		/// Fetches the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<IFeatureRule> GetEnumerator()
		{
			return rules.GetEnumerator();
		}

		/// <summary>
		/// Fetches the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
