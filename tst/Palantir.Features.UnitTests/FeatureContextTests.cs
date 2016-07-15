namespace Palantir.Features.UnitTests
{
	using FluentAssertions;
	using Testing;
	using Xunit;

	[Category("CI"), Time("Fast")]
	public sealed class FeatureContextTests
    {
		[Fact]
		public void FeatureContext_WhenMissing_ShouldReturnNull()
		{
			var context = new FeatureContext();

			context["Test"].Should().BeNull();
		}

		[Fact]
		public void FeatureContext_WhenAdded_ShouldReturnValue()
		{
			var context = new FeatureContext();

			context.Add("Test", "ABC");
			context["Test"].Should().Be("ABC");
		}
	}
}
