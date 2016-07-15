namespace Palantir.Health
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// A collection of health reports.
	/// </summary>
	public sealed class HealthReportCollection
    {
		private readonly IReadOnlyDictionary<string, HealthReport> policyReports;
		private readonly HealthStatus status;

		/// <summary>
		/// Initializes a new instance of the <see cref="HealthReportCollection"/> class.
		/// </summary>
		/// <param name="policyReports">The policy reports.</param>
		public HealthReportCollection(IDictionary<string, HealthReport> policyReports)
		{
			Contract.Requires(policyReports != null);

			this.policyReports = new ReadOnlyDictionary<string, HealthReport>(policyReports);
			status = (from i in policyReports.Values
						   orderby i.Status descending
						   select i.Status).FirstOrDefault();
		}

		/// <summary>
		/// The health status. Unknown if no indicators were found.
		/// </summary>
		public HealthStatus Status => status;

		/// <summary>
		/// The reports for each individual policy.
		/// </summary>
		public IReadOnlyDictionary<string, HealthReport> PolicyReports => policyReports;
    }
}
