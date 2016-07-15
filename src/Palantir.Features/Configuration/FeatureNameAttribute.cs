namespace Palantir.Features.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Indicates the name of a feature.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class FeatureNameAttribute : Attribute
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="FeatureNameAttribute"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public FeatureNameAttribute(string name)
		{
			Contract.Requires(!string.IsNullOrEmpty(name));

			Name = name;
		}

		public string Name { get; }
    }
}
