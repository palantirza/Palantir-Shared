namespace Palantir.Features.UnitTests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Xunit;
	using FluentAssertions;
	using NSubstitute;
	using Microsoft.Extensions.Options;
	using Configuration;
	using System.Collections.ObjectModel;
	using Features.Rules;
	using Serilog;

	public class OptionsModelFeatureConfigurationTests
    {
		public class MyFeatures
		{
			public bool FeatureA { get; }
			public bool FeatureB { get; }

			[FeatureName("feature-c")]
			public bool FeatureC { get; }

			public bool FeatureD { get; }

			[Obsolete("Test", false)]
			public bool SoftObsolete { get; }

			[Obsolete("Test", true)]
			public bool HardObsolete { get; }
		}

		private MyFeatures features = new MyFeatures();
		private IOptions<FeaturesOptions<MyFeatures>> options;
		private OptionsModelFeatureConfiguration<MyFeatures> configService;
		private FeaturesOptions<MyFeatures> config;
		private ILogger logger;

		public OptionsModelFeatureConfigurationTests()
		{
			config = new FeaturesOptions<MyFeatures>
			{
				Features = new Collection<FeatureOptions>
				{
					new FeatureOptions
					{
						Name = "MyFeatures.FeatureB",
						Rules = new Collection<RuleOptions>
						{
							new RuleOptions { Type = "Palantir.Features.Rules.FeatureEnabledRule, Palantir.Features", Enabled = true }
						}
					},
					new FeatureOptions
					{
						Name = "feature-c",
						Rules = new Collection<RuleOptions>
						{
							new RuleOptions { Type = "Palantir.Features.Rules.FeatureEnabledRule, Palantir.Features", Enabled = true }
						}
					},
					new FeatureOptions
					{
						Name = "MyFeatures.FeatureD",
						Rules = new Collection<RuleOptions>
						{
							new RuleOptions { Type = "enabled", Enabled = true }
						}
					},
					new FeatureOptions
					{
						Name = "MyFeatures.SoftObsolete",
						Rules = new Collection<RuleOptions>
						{
							new RuleOptions { Type = "enabled", Enabled = true }
						}
					},
					new FeatureOptions
					{
						Name = "MyFeatures.HardObsolete",
						Rules = new Collection<RuleOptions>
						{
							new RuleOptions { Type = "enabled", Enabled = true }
						}
					}
				}
			};
			options = Substitute.For<IOptions<FeaturesOptions<MyFeatures>>>();
			options.Value.Returns(config);
			logger = Substitute.For<ILogger>();

			configService = new OptionsModelFeatureConfiguration<MyFeatures>(options, logger);
		}

		[Fact]
		public void FeatureConfigGetRules_WhenNoRule_ReturnNull()
		{
			var result = configService.AsyncGetFeatureRules(x => x.FeatureA).Result;

			result.Should().NotBeNull();
			result.Should().BeEmpty();
		}

		[Fact]
		public void FeatureConfigGetRules_WhenMatchingRule_ReturnRules()
		{
			var result = configService.AsyncGetFeatureRules(x => x.FeatureB).Result;

			result.Should().NotBeNull();
			result.Count().Should().Be(1);
			var rule = result.Single().As<FeatureEnabledRule>();
			rule.Should().NotBeNull();
			rule.Enabled.Should().BeTrue();
		}

		[Fact]
		public void FeatureConfigGetRules_WithTypeName_ReturnRules()
		{
			var result = configService.AsyncGetFeatureRules("MyFeatures.FeatureB").Result;

			result.Should().NotBeNull();
			result.Count().Should().Be(1);
			var rule = result.Single().As<FeatureEnabledRule>();
			rule.Should().NotBeNull();
			rule.Enabled.Should().BeTrue();
		}

		[Fact]
		public void FeatureConfigGetRules_NameFeature_ReturnRules()
		{
			var result = configService.AsyncGetFeatureRules(x => x.FeatureC).Result;

			result.Should().NotBeNull();
			result.Count().Should().Be(1);
			var rule = result.Single().As<FeatureEnabledRule>();
			rule.Should().NotBeNull();
			rule.Enabled.Should().BeTrue();
		}

		[Fact]
		public void FeatureConfigGetRules_WithNamedType_ReturnRules()
		{
			var result = configService.AsyncGetFeatureRules(x => x.FeatureD).Result;

			result.Should().NotBeNull();
			result.Count().Should().Be(1);
			var rule = result.Single().As<FeatureEnabledRule>();
			rule.Should().NotBeNull();
			rule.Enabled.Should().BeTrue();
		}

		[Fact]
		public void FeatureConfigGetRules_NameFeatureWithTextName_ReturnRules()
		{
			var result = configService.AsyncGetFeatureRules("feature-c").Result;

			result.Should().NotBeNull();
			result.Count().Should().Be(1);
			var rule = result.Single().As<FeatureEnabledRule>();
			rule.Should().NotBeNull();
			rule.Enabled.Should().BeTrue();
		}

		[Fact]
		public void FeatureConfigGetRules_NameFeatureWithTypeName_ReturnNothing()
		{
			var result = configService.AsyncGetFeatureRules("MyFeatures.FeatureC").Result;

			result.Should().NotBeNull();
			result.Count().Should().Be(0);
		}

		[Fact]
		public void FeatureConfigGetRules_WithObsoleteRule_ShouldLogError()
		{
			var result = configService.AsyncGetFeatureRules("MyFeatures.SoftObsolete").Result;

			logger.Received().Warning(Arg.Any<string>(), Arg.Any<object[]>());
		}

		[Fact]
		public void FeatureConfigGetRules_WithHardObsoleteRule_ShouldLogError()
		{
			Action action = () => configService.AsyncGetFeatureRules("MyFeatures.HardObsolete").Wait();

			action.ShouldThrow<ObsoleteFeatureException>();
			logger.Received().Error(Arg.Any<string>(), Arg.Any<object[]>());
		}
	}
}
