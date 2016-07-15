using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Palantir.Testing
{
	/// <summary>
	/// This class discovers all of the tests and test classes that have
	/// applied the Time attribute
	/// </summary>
	public class TimeDiscoverer : ITraitDiscoverer
    {
		/// <summary>
		/// Gets the trait values from the Time attribute.
		/// </summary>
		/// <param name="traitAttribute">The trait attribute containing the trait values.</param>
		/// <returns>The trait values.</returns>
		public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
		{
			var ctorArgs = traitAttribute.GetConstructorArguments().ToList();
			yield return new KeyValuePair<string, string>("Time", ctorArgs[0].ToString());
		}
	}
}
