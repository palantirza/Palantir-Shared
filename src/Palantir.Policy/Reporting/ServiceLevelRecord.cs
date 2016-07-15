namespace Palantir.Policy.Reporting
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Represents the report for a service level.
	/// </summary>
    public sealed class ServiceLevelRecord
    {
		private readonly ServiceLevelTiming duration;
		private readonly double mtbf;
		private readonly double mttr;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceLevelRecord"/> class.
		/// </summary>
		/// <param name="duration">The duration.</param>
		/// <param name="mtbf">The mean time before failure.</param>
		public ServiceLevelRecord(ServiceLevelTiming duration, double mtbf, double mttr)
		{
			Contract.Requires(duration != null);

			this.duration = duration;
			this.mtbf = mtbf;
			this.mttr = mttr;
		}
		
		/// <summary>
		/// The duration service level information.
		/// </summary>
		public ServiceLevelTiming Duration => duration;

		/// <summary>
		/// The Mean time before failure.
		/// </summary>
		public double MeanTimeBeforeFailure => mtbf;

		/// <summary>
		/// The Mean time to recovery.
		/// </summary>
		public double MeanTimeToRecoveryFailure => mttr;
	}
}
