// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Palantir (Pty) Ltd. [2013] - [2014]">
//   All Rights Reserved.
//   NOTICE:  All information contained herein is, and remains
//   the property of Palantir (Pty) Ltd. and its suppliers,
//   if any.  The intellectual and technical concepts contained
//   herein are proprietary to Palantir (Pty) Ltd.
//   and its suppliers and may be covered by U.S. and Foreign Patents,
//   patents in process, and are protected by trade secret or copyright law.
//   Dissemination of this information or reproduction of this material
//   is strictly forbidden unless prior written permission is obtained
//   from Palantir (Pty) Ltd.
// </copyright>
// <summary>
//   Marks a type as immutable.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Palantir
{
	using System;

	/// <summary>
	/// Marks a type as immutable.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
	public sealed class ImmutableAttribute : Attribute
	{
		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Palantir.ImmutableAttribute"/> is strictly immutable.
		/// </summary>
		/// <value><c>true</c> if strict; otherwise, <c>false</c>.</value>
		public bool Strict { get; set; }

		#endregion
	}
}