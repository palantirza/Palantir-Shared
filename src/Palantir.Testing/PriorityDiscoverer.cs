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
	/// applied the Priority attribute
	/// </summary>
	public class PriorityDiscoverer : ITraitDiscoverer
    {
		/// <summary>
		/// Gets the trait values from the Priority attribute.
		/// </summary>
		/// <param name="traitAttribute">The trait attribute containing the trait values.</param>
		/// <returns>The trait values.</returns>
		public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
		{
			var ctorArgs = traitAttribute.GetConstructorArguments().ToList();
			yield return new KeyValuePair<string, string>("Priority", ctorArgs[0].ToString());
		}
	}
}
