namespace Palantir
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Used to create instances of interfaces.
	/// </summary>
	public static class EventCreator
    {
		/// <summary>
		/// Create an instance of an interface.
		/// </summary>
		/// <typeparam name="T">The event type/</typeparam>
		/// <param name="init">Initializes the interface.</param>
		/// <returns>The initialized interface implementation.</returns>
		public static T CreateInstanceOf<T>(Action<T> init = null)
		{
			var obj = FakeItEasy.A.Fake<T>();

			if (init != null)
				init(obj);

			return obj;
		}

	}
}
