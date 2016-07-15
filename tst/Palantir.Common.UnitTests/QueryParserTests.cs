namespace Palantir.Common.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Xunit;
	using FluentAssertions;
	using Search;
	public sealed class QueryParserTests
    {
		private class TargetType
		{
			public string Text { get; set; }
			public bool IsActive { get; set; }
		}

		[Fact]
		public void SimpleQuery_ShouldReturnMatch()
		{
			var parser = new QueryParser();
			var query = parser.Parse("Test");

			query.Match.Should().Be("Test");
		}

		[Fact]
		public void QueryWithProperty_ShouldReturnMatch()
		{
			var parser = new QueryParser();
			var query = parser.Parse("Test IsActive:true");

			query.Match.Should().Be("Test");
			query.Criteria.ContainsKey("IsActive").Should().BeTrue();
			query.Criteria["IsActive"].Count.Should().Be(1);
			query.Criteria["IsActive"].Index(0).Should().Be("true");
		}

		[Fact]
		public void QueryWithListProperty_ShouldReturnMatch()
		{
			var parser = new QueryParser();
			var query = parser.Parse("Test Status:{Planned,Active}");

			query.Match.Should().Be("Test");
			query.Criteria.ContainsKey("Status").Should().BeTrue();
			query.Criteria["Status"].Count.Should().Be(1);
			query.Criteria["Status"].Index(0).Should().Be("{Planned,Active}");
		}
	}
}
