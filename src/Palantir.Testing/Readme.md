## Attributes
Attributes to tag tests or test classes.

* Category - The test category, e.g. "CI"
* Priority - The test priority, e.g. "High"
* Time - The test speed, e.g. "Slow"

## TestBases

Base classes for test classes based on the type of item they are testing. The all use DependencyInjection to provide the 
fakes to the test class, and all have a property where the constructed object under test can be accessed, initialized with it's
dependencies:

* ValidatorTestBase - Used to test FluentValidators
* ControllerTestBase - Used to test MVC controllers
* HandlerTestBase - Used to test Palantir EventStore event handlers

```
	[Category("CI"), Time("Fast")]
	public sealed class MyControllerTests : ControllerTestBase<MyController>
    {
		public ReleaseTests()
		{
			UseServices(services =>
			{
				services.AddSingleton(x => Substitute.For<IRepository>()); // Add the repository mock to the dependency injection
			});
		}

		[Fact]
		public void MyControllerGet_WhenValid_ShouldReturnResults()
		{
          // Set up the repository mock
			GetService<IRepository>()
				.GetItems(null, 1, 50)
				.Returns(Task.FromResult((PagedList.IPagedList<MyItem>) new SerializablePagedList<MyItem>(new[] { new MyItem() }, 1, 50, 1)));

           // Execute the controller
			var result = Controller.Get();

            // Validate the results.
			result.Result.Should().BeOfType<HttpOkObjectResult>();

			var objectResult = result.Result.As<HttpOkObjectResult>().Value;
			objectResult.Should().BeOfType<GetItemResponse>();
			var itemResponse = objectResult.As<GetItemResponse>();
			itemResponse.PageNumber.Should().Be(1);
			itemResponse.PageSize.Should().Be(50);
			itemResponse.TotalItemCount.Should().Be(1);
			itemResponse.Count().Should().Be(1);
		}

}
```