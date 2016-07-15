namespace Palantir.Features
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Represents a feature context, a set of keys and values about the context.
	/// </summary>
	public sealed class FeatureContext : IFeatureContext
	{
		private Dictionary<string, object> _entries = new Dictionary<string, object>();

		/// <summary>
		/// Initialize a new instance of the <see cref="FeatureContext"/> class.
		/// </summary>
		public FeatureContext()
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="FeatureContext"/> class.
		/// </summary>
		/// <param name="properties">
		/// The properties.
		/// </param>
		public FeatureContext(object properties)
		{
			var propertiesType = properties.GetType();
			foreach (var prop in propertiesType.GetProperties())
				_entries.Add(prop.Name, prop.GetValue(properties));
		}

		/// <summary>
		/// Fetches the value for the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value for the key, or null.</returns>
		public object this[string key]
		{
			get
			{
				return _entries.ContainsKey(key) ? _entries[key] : null;
			}
		}

		/// <summary>
		/// Adds a tag and value.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <param name="value">The value.</param>
		public FeatureContext Add(string tag, object value)
		{
			_entries.Add(tag, value);

			return this;
		}
	}
}