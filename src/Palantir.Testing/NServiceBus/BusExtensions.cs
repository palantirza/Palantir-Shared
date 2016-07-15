namespace NServiceBus.Testing
{
	using NSubstitute.Core;
	using NSubstitute.Routing;
	using NServiceBus;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public static class BusExtensions
    {
		public static void VerifyPublish<TEvent>(this ISendOnlyBus bus, Predicate<TEvent> matches) where TEvent: class
		{
			var router = GetRouterForSubstitute(bus);
			var routeFactory = RouteFactory();

			bool found = false;
			foreach (var call in router.ReceivedCalls())
			{
				var methodInfo = call.GetMethodInfo();
				if (methodInfo.Name == "Publish")
				{
					// Ensure of correct type
					if (methodInfo.GetGenericArguments()[0] == typeof(TEvent))
					{
						TEvent ev;
						if (methodInfo.GetParameters()[0].ParameterType.Name == "Action`1")
						{
							ev = FakeItEasy.A.Fake<TEvent>();
							var act = (Action<TEvent>)call.GetArguments()[0];
							act(ev);
						}
						else
						{
							ev = (TEvent)call.GetArguments()[0];
						}

						found = true;

						var result = matches(ev);

						if (!result)
							throw new ArgumentException($"Published event {typeof(TEvent).Name} does not match predicate");
					}
				}
			}

			if (!found)
				throw new ArgumentException($"Event {typeof(TEvent).Name} not published");
		}

		private static ICallRouter GetRouterForSubstitute<T>(T substitute)
		{
			var context = SubstitutionContext.Current;
			return context.GetCallRouterFor(substitute);
		}
		private static IRouteFactory RouteFactory()
		{
			return SubstitutionContext.Current.GetRouteFactory();
		}
	}
}
