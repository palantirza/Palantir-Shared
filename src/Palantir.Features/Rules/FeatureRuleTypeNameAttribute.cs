namespace Palantir.Features.Rules
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Sets the name for a feature rule type.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class FeatureRuleTypeNameAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FeatureRuleTypeNameAttribute"/> class.
		/// </summary>
		/// <param name="name">The rule name.</param>
		public FeatureRuleTypeNameAttribute(string name)
		{
			Contract.Requires(!string.IsNullOrEmpty(name));

			Name = name;
		}

		public string Name { get; }
	}
}
