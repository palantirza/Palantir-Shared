namespace Palantir.Features
{
	using Configuration;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Extension methods for IServiceCollection.
	/// </summary>
	public static class ServiceCollectionExtensions
    {
		/// <summary>
		/// Configures the features.
		/// </summary>
		/// <typeparam name="TFeatures">The feature type to configure.</typeparam>
		/// <param name="services">The services collection.</param>
		/// <param name="configSection">The configuration section.</param>
		public static void ConfigureFeatures<TFeatures>(this IServiceCollection services, IConfigurationSection configSection) where TFeatures: class, new()
		{
			Contract.Requires(services != null);
			Contract.Requires(configSection != null);
            
			services.Configure<FeaturesOptions<TFeatures>>(configSection);
			services.AddSingleton<IFeatureConfiguration<TFeatures>, OptionsModelFeatureConfiguration<TFeatures>>();
			services.AddSingleton<IFeatureRouter<TFeatures>, FeatureRouter<TFeatures>>();
			services.AddTransient(s => s.GetService<IFeatureRouter<TFeatures>>().GetFeatureDecisions());
		}
	}
}
