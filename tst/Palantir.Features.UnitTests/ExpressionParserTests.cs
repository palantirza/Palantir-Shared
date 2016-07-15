namespace Palantir.Features.UnitTests
{
	using FluentAssertions;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Xunit;

	public sealed class ExpressionParserTests
    {
		public class MyFeature
		{
			public bool FeatureA { get; }
		}

		[Fact]
		public void GetFeatureName_ShouldReturnQualifiedName()
		{
			var name = ExpressionParser.GetQualifiedName<MyFeature>(x => x.FeatureA);
			name.Should().Be("MyFeature.FeatureA");
		}
    }
}
