namespace Palantir.Health
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// The health status.
	/// </summary>
	public enum HealthStatus
	{
		Unknown = 0,
		OK = 1,
		Warning = 2,
		Error = 3
	}

	/// <summary>
	/// Information about the health of an indicator.
	/// </summary>
    public sealed class Health
    {
		private readonly string name;
		private readonly HealthStatus status;
		private readonly string statusText;
		private readonly object detail;

		/// <summary>
		/// Initializes a new instance of the <see cref="Health"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="status">The status.</param>
		/// <param name="statusText">The status text.</param>
		/// <param name="detail">The health detail.</param>
		public Health(string name, HealthStatus status, string statusText = null, object detail = null)
		{
			Contract.Requires(!string.IsNullOrEmpty(name));
			Contract.Requires(Enum.IsDefined(typeof(HealthStatus), status));

			this.name = name;
			this.status = status;
			this.statusText = statusText;
			this.detail = detail;
		}

		/// <summary>
		/// The name of the indicator.
		/// </summary>
		public string Name => name;

		/// <summary>
		/// The status of the indicator.
		/// </summary>
		public HealthStatus Status => status;

		/// <summary>
		/// The status text.
		/// </summary>
		public string StatusText => statusText;

		/// <summary>
		/// Any detail information about the health.
		/// </summary>
		public object Detail => detail;
    }
}
