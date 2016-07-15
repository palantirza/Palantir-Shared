namespace Palantir.Policy.Reporting
{
	using HdrHistogram;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// A record of a set of service level timing values.
	/// </summary>
	public sealed class ServiceLevelTiming
    {
		private readonly long count;
		private readonly long min;
		private readonly long max;
		private readonly double mean;
		private readonly double stddev;
		private readonly ReadOnlyDictionary<double, long> percentiles;

		/// <summary>
		/// Initializes a new instance of the ServiceLevelRecord.
		/// </summary>
		/// <param name="histogram">The histogram.</param>
		internal ServiceLevelTiming(LongHistogram histogram)
		{
			Contract.Requires(histogram != null);

			min = histogram.LowestEquivalentValue(histogram.RecordedValues().Select(hiv => hiv.ValueIteratedTo).FirstOrDefault());
			max = histogram.GetMaxValue();
			mean = histogram.GetMean();
			stddev = histogram.GetStdDeviation();
			count = histogram.TotalCount;

			var percentiles = new Dictionary<double, long>();
			percentiles.Add(50, histogram.GetValueAtPercentile(50));
			percentiles.Add(90, histogram.GetValueAtPercentile(90));
			percentiles.Add(95, histogram.GetValueAtPercentile(95));
			percentiles.Add(99, histogram.GetValueAtPercentile(99));
			percentiles.Add(99.9, histogram.GetValueAtPercentile(99.9));
			percentiles.Add(99.99, histogram.GetValueAtPercentile(99.99));

			this.percentiles = new ReadOnlyDictionary<double, long>(percentiles);
		}

		public long Minimum => min;
		public long Maximum => max;
		public double Mean => mean;
		public double StandardDeviation => stddev;
		public double Count => count;
		public IReadOnlyDictionary<double, long> Percentiles => percentiles;
	}
}
