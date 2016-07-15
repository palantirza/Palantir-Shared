# Palantir Shared Libraries

A collection of useful utilities and code we've collected.

## Palantir.Common

### ISystemContext
A mechanism to pass system contextual information to your classes. At this point in time, just the User and the current time.

### EnumerableExtensions
Some useful extensions for IEnumerable

* **NullToEmpty** - If the source is null, returns an empty enumerable, otherwise returns the enumerable. Shorthand function.
* **FindIndex** - Find the index of a matching predicate in an enumerable.
* **IndexOf** - Find the index of an item in an enumerable.
* **Index** - Returns the item at the specified index.

### EventCreator
Helper to create implementations of interfaces at runtime for DTO objects.

### ImmutableAttribute
Used to indicate that your class is immutable. Does nothing, just metadata for now. Intention is to hook convention tests into
it later. Strict means that event Reflection can't change your data, so you have readonly fields only.

### LoggingExtensions
Allows you to log named events to Serilog. Also, `With` adds the ability to add custom metadata to a sset of log entries.

### Paging
A set of classes to work with PagedLists better. A `SerializablePagedList` and async helper methods to populate it.

## Palantir.Testing

A set of helpers for testing.

### Bus Extensions

One very useful method for NServiceBus testing: VerifyPublish

### ControllerTestBase

A base class that builds a controller instance via Dependency Injection. Use `UseServices` in the constructor to set up the services. Then just reference `Controller` and test it.

### Test Attributes

* **Category** - Specify the test category
* **Priority** - Specify the test priority
* **Time** - Specify the test time, e.g. "Fast", "Slow"

### ValidatorTestBase

A base class that builds a FluentValidator instance via Dependency Injection. Use `UseServices` in the constructor to set up the services. Then just reference `Validator` and test it.

## Palantir.Features

A powerful and extensible feature toggle library based on [Martin Fowler's article](http://martinfowler.com/articles/feature-toggles.html), and incorporating into the design the ability to report on and control feature toggles remotely. Read it's README for more detail.

## Palantir.Health

A health check library based on Swing as an inspiration. Read it's README for more detail.