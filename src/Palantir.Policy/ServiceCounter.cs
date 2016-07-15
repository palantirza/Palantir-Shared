namespace Palantir.Policy
{
	using HdrHistogram;
	using System;
	using System.Threading;
	using Reporting;

	/// <summary>
	/// The service level for a service.
	/// </summary>
	internal sealed class ServiceCounter
	{
		private ReaderWriterLockSlim durationHistogramLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
		private LongHistogram durationHistogram = new LongHistogram(TimeSpan.TicksPerHour / TimeSpan.TicksPerMillisecond, 3);
		private LongHistogram successHistogram = new LongHistogram(TimeSpan.TicksPerHour / TimeSpan.TicksPerMillisecond, 3);
		private LongHistogram failureHistogram = new LongHistogram(TimeSpan.TicksPerHour / TimeSpan.TicksPerMillisecond, 3);
		private ServiceStatus status = null;

		private class ServiceStatus
		{
			public DateTimeOffset StatusStartTimeStamp { get; set; }
			public bool Successful { get; set; }
		}

		/// <summary>
		/// Reports the duration for a service level.
		/// </summary>
		/// <param name="duration">The duration to report.</param>
		/// <returns>The service level.</returns>
		public ServiceCounter ReportSuccess(long duration, DateTimeOffset timestamp)
		{
			durationHistogramLock.EnterWriteLock();
			try
			{
				durationHistogram.RecordValue(duration);

				if (status == null)
					status = new ServiceStatus { StatusStartTimeStamp = timestamp, Successful = true };
				else if (!status.Successful)
				{
					var timeBeforeRecovery = (timestamp - status.StatusStartTimeStamp).TotalMilliseconds;
					failureHistogram.RecordValue((long)timeBeforeRecovery);

					status = new ServiceStatus { StatusStartTimeStamp = timestamp, Successful = true };
				}
			}
			finally
			{
				durationHistogramLock.ExitWriteLock();
			}
			return this;
		}

		/// <summary>
		/// Reports the duration for a service level.
		/// </summary>
		/// <param name="duration">The duration to report.</param>
		/// <returns>The service level.</returns>
		public ServiceCounter ReportFailure(long duration, DateTimeOffset timestamp)
		{
			durationHistogramLock.EnterWriteLock();
			try
			{
				durationHistogram.RecordValue(duration);

				if (status == null)
					status = new ServiceStatus { StatusStartTimeStamp = timestamp, Successful = false };
				else if (!status.Successful)
				{
					var timeBeforeFailure = (timestamp - status.StatusStartTimeStamp).TotalMilliseconds;
					successHistogram.RecordValue((long)timeBeforeFailure);

					status = new ServiceStatus { StatusStartTimeStamp = timestamp, Successful = false };
				}
			}
			finally
			{
				durationHistogramLock.ExitWriteLock();
			}
			return this;
		}

		public ServiceLevelRecord GetRecord()
		{
			durationHistogramLock.EnterReadLock();
			try
			{
				return new ServiceLevelRecord(new ServiceLevelTiming(durationHistogram), successHistogram.GetMean(), failureHistogram.GetMean());
			}
			finally
			{
				durationHistogramLock.ExitReadLock();
			}
		}
	}
}
