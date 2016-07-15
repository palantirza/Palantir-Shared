namespace Palantir.Features.UnitTests
{
	using Features.Rules;
	using FluentAssertions;
	using NSubstitute;
	using Serilog;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Xunit;

	public sealed class FeatureRouterTests
    {
		public class MyFeatures
		{
			public bool FeatureA { get; set; }
		}

		private FeatureRouter<MyFeatures> router;
		private IFeatureConfiguration<MyFeatures> config;
		private ILogger log;

		public FeatureRouterTests()
		{
			config = Substitute.For<IFeatureConfiguration<MyFeatures>>();
			log = Substitute.For<ILogger>();
			router = new FeatureRouter<MyFeatures>(config, log);
		}

		[Fact]
		public void FeatureRouter_WithNoRules_ShouldBeFalse()
		{
			config.AsyncGetFeatureRules(Arg.Any<string>()).Returns(Task.FromResult((IEnumerable<IFeatureRule>)new IFeatureRule[] { }));

			router.AsyncIsFeatureEnabled(x => x.FeatureA).Result.Should().BeFalse();
		}

		[Fact]
		public void FeatureRouter_WithDisabledRule_ShouldBeFalse()
		{
			config.AsyncGetFeatureRules(Arg.Any<string>()).Returns(Task.FromResult((IEnumerable<IFeatureRule>)new IFeatureRule[] { FeatureEnabledRule.FeatureDisabled }));

			router.AsyncIsFeatureEnabled(x => x.FeatureA).Result.Should().BeFalse();
		}

		[Fact]
		public void FeatureRouter_WithEnabledRule_ShouldBeFalse()
		{
			config.AsyncGetFeatureRules(Arg.Any<string>()).Returns(Task.FromResult((IEnumerable<IFeatureRule>)new IFeatureRule[] { FeatureEnabledRule.FeatureEnabled }));

			router.AsyncIsFeatureEnabled(x => x.FeatureA).Result.Should().BeTrue();
		}

		[Fact]
		public void FeatureRouter_GetDescision_ShouldReturnDecision()
		{
			config.AsyncGetFeatureRules(Arg.Any<string>()).Returns(Task.FromResult((IEnumerable<IFeatureRule>)new IFeatureRule[] { FeatureEnabledRule.FeatureEnabled }));

			var decision = router.AsyncGetFeatureDecisions().Result;
			decision.Should().NotBeNull();
			decision.FeatureA.Should().BeTrue();
		}
	}
}
