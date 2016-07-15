namespace Palantir.Policy.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Xunit;
	using FluentAssertions;
	using HdrHistogram;
	public sealed class ServiceCounterUnitTests
    {
		[Fact]
		public void TestSimpleMTBF()
		{
			var counter = new ServiceCounter();
			counter.ReportSuccess(50, new DateTimeOffset(new DateTime(2010, 1, 1, 1, 0, 0)));
			counter.ReportSuccess(50, new DateTimeOffset(new DateTime(2010, 1, 1, 1, 10, 0)));
			counter.ReportFailure(50, new DateTimeOffset(new DateTime(2010, 1, 1, 1, 20, 0)));

			var record = counter.GetRecord();

			record.MeanTimeBeforeFailure.Should().Be(20);
		}

		[Fact]
		public void TestHistogram()
		{
			var x = TimeSpan.TicksPerHour / TimeSpan.TicksPerSecond;
			LongHistogram durationHistogram = new LongHistogram(TimeSpan.TicksPerHour / TimeSpan.TicksPerMillisecond, 5);
			durationHistogram.RecordValue(500000);
		}
	}
}
