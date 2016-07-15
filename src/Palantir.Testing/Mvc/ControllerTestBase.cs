namespace Palantir.Testing.Mvc
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.DependencyInjection;
    using NSubstitute;

    public abstract class ControllerTestBase<TController> : ServiceProviderTestBase where TController : Controller
    {
		private TController _controller;
		private HttpContext _httpContext = Substitute.For<HttpContext>();
		private IUrlHelper _urlHelper = Substitute.For<IUrlHelper>();
		private ITempDataDictionary _tempDataDictionary = Substitute.For<ITempDataDictionary>();

		public ControllerTestBase()
		{
			Services.AddTransient<TController>();
		}

		protected TController Controller
		{
			get
			{
				EnsureCreated();

				return _controller;
			}
		}

		protected HttpContext HttpContextMock => _httpContext;

		protected IUrlHelper UrlMock => _urlHelper;

		protected ITempDataDictionary TempDataMock => _tempDataDictionary;

		protected override void EnsureCreated()
		{
			if (_controller == null)
			{
				BuildServiceProvider();
				
				_controller = InternalGetService<TController>();
                _controller.ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContext
                };
				_controller.Url = _urlHelper;
				_controller.TempData = _tempDataDictionary;
			}
		}

	}
}
