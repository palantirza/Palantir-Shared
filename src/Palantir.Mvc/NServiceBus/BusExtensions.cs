namespace Palantir.Mvc.NServiceBus
{
	using global::NServiceBus;
	using System.Diagnostics.Contracts;

	/// <summary>
	/// NServiceBus extensions.
	/// </summary>
	public static class BusExtensions
    {
#if DNX46
		/// <summary>
		/// Inject correlation information into the message.
		/// </summary>
		/// <param name="message">The message.</param>
		public static void InjectCorrelation(this ICorrelatedEvent message)
		{
			Contract.Requires(message != null);

			message.CorrelationId = ActivityScope.Current?.Id;
		}
#endif
    }
}
