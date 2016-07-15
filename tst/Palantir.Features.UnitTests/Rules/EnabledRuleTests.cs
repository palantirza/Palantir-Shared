namespace Palantir.Features.UnitTests.Rules
{
	using Features.Rules;
	using FluentAssertions;
	using Xunit;

	public sealed class EnabledRuleTests
    {
		[Fact]
		public void EnabledRule_ShouldEvaluateTrue()
		{
			var rule = new FeatureEnabledRule(true);

			rule.EvaluateRule(null).Should().BeTrue();
		}

		[Fact]
		public void DisabledRule_ShouldEvaluateTrue()
		{
			var rule = new FeatureEnabledRule(false);

			rule.EvaluateRule(null).Should().BeFalse();
		}
	}
}
