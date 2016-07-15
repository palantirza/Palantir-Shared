// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SR.cs" company="Palantir (Pty) Ltd. [2013] - [2014]">
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
//   The sr.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Palantir.Common;

namespace Palantir
{
	/// <summary>
	/// The sr.
	/// </summary>
	internal static class SR
	{
		#region Public Methods and Operators
		
		/// <summary>
		/// Invalid format for the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The error text.</returns>
		public static string Err_InvalidFormatFor(string type)
		{
			return string.Format(Resource.Err_InvalidFormatFor, type);
		}

		/// <summary>
		/// The event message cannot be a concrete type, it must be an interface.
		/// </summary>
		/// <param name="eventType">The event type.</param>
		/// <returns>The error text.</returns>
		public static string Err_EventMessageCannotBeConcreteType(Type eventType)
		{
			return string.Format(Resource.Err_EventMessageCannotBeConcreteType, eventType.FullName);
		}


        /// <summary>
        /// The paaged list async methods require raven queryables.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <returns>The error text.</returns>
        internal static string Err_PagedListAsyncRequiresRaven()
        {
            return string.Format(Resource.Err_PagedListAsyncRequiresRaven);
        }

        /// <summary>
        /// The event message is not of the indicated type.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <param name="indicatedType">The indicated type.</param>
        /// <returns>The error text.</returns>
        public static string Err_EventMessageNotOfIndicatedType(Type eventType, Type indicatedType)
		{
			return string.Format(Resource.Err_EventMessageNotOfIndicatedType, eventType.FullName, indicatedType.FullName);
		}

		/// <summary>
		/// The event stream has mutated since it was hived off the store.
		/// </summary>
		/// <returns>The error text.</returns>
		public static string Err_EventStreamChangedSinceCreation()
		{
			return string.Format(Resource.Err_EventStreamChangedSinceCreation);
		}

		#endregion
	}
}