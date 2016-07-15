namespace Palantir.Features
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Indicates that a feature is obsolete.
	/// </summary>
	public sealed class ObsoleteFeatureException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ObsoleteFeatureException"/> class.
		/// </summary>
		/// <param name="message">The error message.</param>
		public ObsoleteFeatureException(string message) 
			: base(message)
		{
		}
	}
}
