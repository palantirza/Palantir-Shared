namespace Palantir.Features
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Specifies what "feature" a class or method requires. The version indicates which
	/// version of a feature the class or method belongs to.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class RequiresFeatureAttribute : Attribute
    {
		/// <summary>
		/// Initializes the <see cref="RequiresFeatureAttribute"/> class.
		/// </summary>
		/// <param name="feature">The name of the feature that is required.</param>
		public RequiresFeatureAttribute(string feature)
		{

		}
    }
}
