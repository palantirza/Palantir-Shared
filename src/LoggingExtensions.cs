namespace Palantir
{
	using Serilog;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Extensions for Serilog logging.
	/// </summary>
	public static class LoggingExtensions
    {
		/// <summary>
		/// Logs an information event.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="eventId">The event ID.</param>
		/// <param name="template">The template.</param>
		/// <param name="propertyValues">The property values.</param>
		public static void InformationEvent(this ILogger log, string eventId, string template, params object[] propertyValues)
		{
			var allProps = new object[] { eventId }.Concat(propertyValues).ToArray();
			log.Information("<{EventID:l}> " + template, allProps);
		}

		/// <summary>
		/// Logs an information event.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="eventId">The event ID.</param>
		/// <param name="transactionId">The transation ID></param>
		/// <param name="template">The template.</param>
		/// <param name="propertyValues">The property values.</param>
		public static void InformationEvent(this ILogger log, string eventId, string transactionId, string template, params object[] propertyValues)
		{
			log.ForContext("ContextID", transactionId).InformationEvent(eventId, template, propertyValues);
		}

		/// <summary>
		/// Logs a verbose event.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="eventId">The event ID.</param>
		/// <param name="transactionId">The transation ID></param>
		/// <param name="template">The template.</param>
		/// <param name="propertyValues">The property values.</param>
		public static void VerboseEvent(this ILogger log, string eventId, string template, params object[] propertyValues)
		{
			var allProps = new object[] { eventId }.Concat(propertyValues).ToArray();
			log.Verbose("<{EventID:l}> " + template, allProps);
		}

		/// <summary>
		/// Logs a verbose event.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="eventId">The event ID.</param>
		/// <param name="transactionId">The transation ID></param>
		/// <param name="template">The template.</param>
		/// <param name="propertyValues">The property values.</param>
		public static void VerboseEvent(this ILogger log, string eventId, string transactionId, string template, params object[] propertyValues)
		{
			log.ForContext("ContextID", transactionId).VerboseEvent(eventId, template, propertyValues);
		}

		/// <summary>
		/// Logs a debug event.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="eventId">The event ID.</param>
		/// <param name="transactionId">The transation ID></param>
		/// <param name="template">The template.</param>
		/// <param name="propertyValues">The property values.</param>
		public static void DebugEvent(this ILogger log, string eventId, string template, params object[] propertyValues)
		{
			var allProps = new object[] { eventId }.Concat(propertyValues).ToArray();
			log.Debug("<{EventID:l}> " + template, allProps);
		}

		/// <summary>
		/// Logs a debug event.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="eventId">The event ID.</param>
		/// <param name="transactionId">The transation ID></param>
		/// <param name="template">The template.</param>
		/// <param name="propertyValues">The property values.</param>
		public static void DebugEvent(this ILogger log, string eventId, string transactionId, string template, params object[] propertyValues)
		{
			log.ForContext("ContextID", transactionId).DebugEvent(eventId, template, propertyValues);
		}

		/// <summary>
		/// Logs a warning event.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="eventId">The event ID.</param>
		/// <param name="transactionId">The transation ID></param>
		/// <param name="template">The template.</param>
		/// <param name="propertyValues">The property values.</param>
		public static void WarningEvent(this ILogger log, string eventId, string template, params object[] propertyValues)
		{
			var allProps = new object[] { eventId }.Concat(propertyValues).ToArray();
			log.Warning("<{EventID:l}> " + template, allProps);
		}

		/// <summary>
		/// Logs a warning event.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="eventId">The event ID.</param>
		/// <param name="transactionId">The transation ID></param>
		/// <param name="template">The template.</param>
		/// <param name="propertyValues">The property values.</param>
		public static void WarningEvent(this ILogger log, string eventId, string transactionId, string template, params object[] propertyValues)
		{
			log.ForContext("ContextID", transactionId).WarningEvent(eventId, template, propertyValues);
		}

		/// <summary>
		/// Logs an error event.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="eventId">The event ID.</param>
		/// <param name="transactionId">The transation ID></param>
		/// <param name="template">The template.</param>
		/// <param name="propertyValues">The property values.</param>
		public static void ErrorEvent(this ILogger log, string eventId, string template, params object[] propertyValues)
		{
			var allProps = new object[] { eventId }.Concat(propertyValues).ToArray();
			log.Error("<{EventID:l}> " + template, allProps);
		}

		/// <summary>
		/// Logs an error event.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="eventId">The event ID.</param>
		/// <param name="transactionId">The transation ID></param>
		/// <param name="template">The template.</param>
		/// <param name="propertyValues">The property values.</param>
		public static void ErrorEvent(this ILogger log, string eventId, string transactionId, string template, params object[] propertyValues)
		{
			log.ForContext("ContextID", transactionId).ErrorEvent(eventId, template, propertyValues);
		}


		/// <summary>
		/// Logs a fatal event.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="eventId">The event ID.</param>
		/// <param name="transactionId">The transation ID></param>
		/// <param name="template">The template.</param>
		/// <param name="propertyValues">The property values.</param>
		public static void FatalEvent(this ILogger log, string eventId, string template, params object[] propertyValues)
		{
			var allProps = new object[] { eventId }.Concat(propertyValues).ToArray();
			log.Fatal("<{EventID:l}> " + template, allProps);
		}


		/// <summary>
		/// Logs a falat event.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="eventId">The event ID.</param>
		/// <param name="transactionId">The transation ID></param>
		/// <param name="template">The template.</param>
		/// <param name="propertyValues">The property values.</param>
		public static void FatalEvent(this ILogger log, string eventId, string transactionId, string template, params object[] propertyValues)
		{
			log.ForContext("ContextID", transactionId).FatalEvent(eventId, template, propertyValues);
		}

		/// <summary>
		/// Adds a property value to the context.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="propertyName">The property name.</param>
		/// <param name="value">The value.</param>
		/// <returns>The logger with the added variable.</returns>
		public static ILogger With(this ILogger logger, string propertyName, object value)
		{
			return logger.ForContext(propertyName, value, true);
		}

	}
}
