namespace Palantir.Mvc.Startup
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Cors.Infrastructure;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.PlatformAbstractions;
    using Microsoft.Extensions.Logging.Debug;
    using Serilog;
    using Swashbuckle.Swagger;
    using Swashbuckle.Swagger.Model;
    using System;

    /// <summary>
    /// Extension methods to simplify startup.
    /// </summary>
    public static class StartupExtensions
	{
		/// <summary>
		/// Builds the default configuration root.
		/// </summary>
		/// <returns>The configuration root.</returns>
		/// <remarks>
		/// Uses appsettings.json, and optional environment specific appsettings.{env.EnvironmentName}.json, and
		/// environment variables.
		/// Also uses the user secrets id in appsettings if in development, and the userSecretsId is null.
		/// </remarks>
		public static IConfigurationRoot CreateDefaultConfiguration(this IHostingEnvironment env, string settingsName = "appsettings", Func<IConfigurationBuilder, IConfigurationBuilder> builderConfig = null)
		{
			// Set up configuration sources.
			var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(settingsName + ".json")
				.AddJsonFile(settingsName + $".{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();

            if (builderConfig != null)
                builder = builderConfig(builder);
            
			return builder.Build();
		}

		/// <summary>
		/// Creates the default Serilog logger.
		/// </summary>
		/// <returns>The logger.</returns>
		public static Serilog.ILogger CreateDefaultSerilogLogger<TStartup>(this IHostingEnvironment env, Func<LoggerConfiguration, LoggerConfiguration> loggerConfig = null)
		{
			var loggerConfiguration = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Trace();

            if (loggerConfig != null)
                loggerConfiguration = loggerConfig(loggerConfiguration);

			Log.Logger = loggerConfiguration.CreateLogger();

			var log = Serilog.Log.Logger
				.ForContext<TStartup>();

			return log;
		}

		/// <summary>
		/// Logs default startup information.
		/// </summary>
		/// <param name="log">The log.</param>
		/// <param name="env">The hosting environment.</param>
		public static void LogDefaultStartupInformation(this Serilog.ILogger log, IHostingEnvironment env)
		{
			log.Debug("Environment: {EnvironmentName}", env.EnvironmentName);
			log.Debug("WebRootPath: {WebRootPath}", env.WebRootPath);
			log.Debug("ApplicationName: {ApplicationName}", env.ApplicationName);
			log.Debug("ContentRootPath: {ContentRootPath}", env.ContentRootPath);
		}

		/// <summary>
		/// Configures swagger.
		/// </summary>
		/// <param name="services">The services.</param>
		/// <param name="infoInit">The initialization function.</param>
		public static void ConfigureSwagger(this IServiceCollection services, Action<Info> infoInit)
		{
			var info = new Info
			{
				Version = "v1",
				Title = $"Unkown Service",
				TermsOfService = "Internal use only"
			};
			infoInit(info);

			services.ConfigureSwagger(info);
		}

		/// <summary>
		/// Configures swagger.
		/// </summary>
		/// <param name="services">The services.</param>
		/// <param name="info">The information.</param>
		public static void ConfigureSwagger(this IServiceCollection services, Info info)
		{
			services.AddSwaggerGen();
			services.ConfigureSwaggerGen(options =>
			{
				options.SingleApiVersion(info);
                options.DescribeAllEnumsAsStrings();
			});
		}

		/// <summary>
		/// Configures swagger.
		/// </summary>
		/// <param name="services">The services.</param>
		/// <param name="infoInit">The initialization function.</param>
		public static void ConfigureSwagger(this IServiceCollection services, string title, string version = "v1", string description = null, string termsOfService = "Internal use only")
		{
			services.ConfigureSwagger(config =>
			{
				config.Title = title;
				config.Version = version;
				config.Description = description;
				config.TermsOfService = termsOfService;
			});
		}

		/// <summary>
		/// Sets up swagger for use.
		/// </summary>
		/// <param name="app">The application.</param>
		public static void UseSwashbuckle(this IApplicationBuilder app, string baseRoute = "swagger/ui")
		{
			app.UseSwagger();
			app.UseSwaggerUi(baseRoute);
		}

		/// <summary>
		/// Uses default logging.
		/// </summary>
		/// <param name="loggerFactory">The logger factory.</param>
		public static void UseDefaultLogging(this ILoggerFactory loggerFactory, IConfigurationRoot configuration)
		{
			loggerFactory.AddConsole(configuration.GetSection("Logging"));
			loggerFactory.AddDebug();
			loggerFactory.AddSerilog();
		}

		/// <summary>
		/// Allows any CORS requests.
		/// </summary>
		/// <param name="services">The service collection.</param>
		/// <param name="policyName">The policy name.</param>
		public static void AllowAnyCors(this IServiceCollection services, string policyName = "CorsPolicy")
		{
			services.AddCors(options =>
			{
				options.AddAllowAnyCors(policyName);
			});
		}

		/// <summary>
		/// Adds an Allow Any CORS requests.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <param name="policyName">The policy name.</param>
		public static void AddAllowAnyCors(this CorsOptions options, string policyName = "CorsPolicy")
		{
			options.AddPolicy(policyName, builder =>
			{
				builder.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowAnyOrigin()
				.AllowCredentials();
			});
		}
	}

}
