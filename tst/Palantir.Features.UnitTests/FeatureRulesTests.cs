namespace Palantir.Features.UnitTests
{
	using FluentAssertions;
	using NSubstitute;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Xunit;
	public sealed class FeatureRulesTests
    {
		private FeatureRules rules = new FeatureRules();

		[Fact]
		public void FeatureRulesAddRule_ShouldAddRuleToCollection()
		{
			var rule = Substitute.For<IFeatureRule>();

			rules.Add(rule);

			rules.Count.Should().Be(1);
			rules.Count().Should().Be(1);
			rules.Single().Should().Be(rule);
		}

		[Fact]
		public void FeatureRulesAddRange_ShouldAddRulesToCollection()
		{
			var rule1 = Substitute.For<IFeatureRule>();
			var rule2 = Substitute.For<IFeatureRule>();

			rules.AddRange(new[] { rule1, rule2 });

			rules.Count.Should().Be(2);
			rules.Count().Should().Be(2);
			rules.First().Should().Be(rule1);
			rules.Last().Should().Be(rule2);
		}

		[Fact]
		public void FeatureRulesRangeInitialize_ShouldAddRulesToCollection()
		{
			var rule1 = Substitute.For<IFeatureRule>();
			var rule2 = Substitute.For<IFeatureRule>();

			rules = new FeatureRules(new[] { rule1, rule2 });

			rules.Count.Should().Be(2);
			rules.Count().Should().Be(2);
			rules.First().Should().Be(rule1);
			rules.Last().Should().Be(rule2);
		}
	}
}
