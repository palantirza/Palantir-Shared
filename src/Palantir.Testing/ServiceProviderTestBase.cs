namespace Palantir.Testing.Mvc
{
	using Microsoft.Extensions.DependencyInjection;
	using NSubstitute;
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq;

	public abstract class ServiceProviderTestBase : IServiceProvider
	{
		private IServiceProvider _serviceProvider;

		protected ServiceProviderTestBase()
		{
			Services = new ServiceCollection();
		}

		protected void UseServices(Action<IServiceCollection> configureServices)
		{
			configureServices(Services);
		}

		protected IServiceCollection Services { get; }

		protected TService GetService<TService>()
		{
			EnsureCreated();

			return InternalGetService<TService>();
		}

		public object GetService(Type serviceType)
		{
			EnsureCreated();

			return _serviceProvider.GetService(serviceType);
		}
        
		protected abstract void EnsureCreated();

		protected void BuildServiceProvider()
		{
			_serviceProvider = Services.BuildServiceProvider();
		}

		protected TService InternalGetService<TService>()
		{
			return _serviceProvider.GetService<TService>();
		}
	}
}
