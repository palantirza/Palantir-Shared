namespace Palantir.Mvc
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.PlatformAbstractions;
    using Microsoft.Owin.Infrastructure;
    using Serilog;
    using Startup;
    using Swashbuckle.Swagger;
    using Swashbuckle.Swagger.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// A startup class which abstracts away the common startup processes so they can be consistent
    /// across all projects.
    /// </summary>
    public abstract class TemplatedStartup
	{
		private Serilog.ILogger log;
		private Info swaggerInfo;

		protected TemplatedStartup(IHostingEnvironment env)
		{
			Configuration = env.CreateDefaultConfiguration();
			log = env.CreateDefaultSerilogLogger<TemplatedStartup>();
			log.InformationEvent("WebStart", "Web starting...");
			log.LogDefaultStartupInformation(env);
		}
		
		public IConfigurationRoot Configuration { get; set; }
		
		protected void EnableSwagger(Info info)
		{
			swaggerInfo = info;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public virtual void ConfigureServices(IServiceCollection services)
		{
			services.ConfigureSwagger(swaggerInfo);

			services.AddSingleton<ISystemClock, SystemClock>();
			services.AddSingleton<ISystemContext, DefaultSystemContext>();
            services.AddSingleton(Log.Logger);

			// Add framework services.
			services.AddMvc(options =>
			{
				ConfigureMvcOptions(options);
			});

			services.AddLogging();
		}
		
		protected virtual void ConfigureMvcOptions(MvcOptions options)
		{
		}
		
		protected virtual void ConfigureAuthentication(IApplicationBuilder app)
		{
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
            if (swaggerInfo != null)
                app.UseSwashbuckle();

			loggerFactory.UseDefaultLogging(Configuration);

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseMiddleware<RequestLoggerMiddleware>();
            
			ConfigureAuthentication(app);

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}

	}
}
