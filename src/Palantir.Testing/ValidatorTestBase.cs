namespace Palantir.Testing
{
	using Microsoft.Extensions.DependencyInjection;
	using System;

	public abstract class ValidatorTestBase<TValidator> where TValidator : class, FluentValidation.IValidator
	{
		private TValidator _validator;
		private IServiceProvider _serviceProvider;

		public ValidatorTestBase()
		{
			Services = new ServiceCollection();
			Services.AddTransient<TValidator>();
		}

		protected void UseServices(Action<IServiceCollection> configureServices)
		{
			configureServices(Services);
		}

		protected TValidator Validator
		{
			get
			{
				EnsureCreated();

				return _validator;
			}
		}

		protected IServiceCollection Services { get; }

		protected TService GetService<TService>()
		{
			EnsureCreated();

			return _serviceProvider.GetService<TService>();
		}

		private void EnsureCreated()
		{
			if (_validator == null)
			{
				_serviceProvider = Services.BuildServiceProvider();

				_validator = _serviceProvider.GetService<TValidator>();
			}
		}

	}
}
