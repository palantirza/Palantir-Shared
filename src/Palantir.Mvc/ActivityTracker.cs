/// <acknowledgements>
/// Adapted from https://github.com/CorrelatorSharp/CorrelatorSharp since it didn't support CoreCLR.
/// </acknowledgements>

#if DNX46
namespace Palantir.Mvc
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;

	/// <summary>
	/// Tracks the acitvities.
	/// </summary>
	internal static class ActivityTracker
    {
		private static readonly AsyncLocal<Stack<ActivityScope>> _activityStack = new AsyncLocal<Stack<ActivityScope>>();

		/// <summary>
		/// The current acitivity scope.
		/// </summary>
		public static ActivityScope Current
		{
			get
			{
				if (_activityStack.Value != null && _activityStack.Value.Count > 0)
					return _activityStack.Value.Peek();

				return null;
			}
		}

		/// <summary>
		/// Finds the activity with the given ID.
		/// </summary>
		/// <param name="id">The ID.</param>
		/// <returns>The activity scope or null.</returns>
		public static ActivityScope Find(string id)
		{
			if (String.IsNullOrWhiteSpace(id))
				return null;

			if (_activityStack.Value != null && _activityStack.Value.Count > 0)
				return _activityStack.Value.FirstOrDefault(scope => String.Equals(id, scope.Id, StringComparison.InvariantCultureIgnoreCase));

			return null;
		}

		/// <summary>
		/// The activity stack.
		/// </summary>
		private static Stack<ActivityScope> ActivityStack
		{
			get
			{
				if (_activityStack.Value == null)
					return _activityStack.Value = new Stack<ActivityScope>();

				return _activityStack.Value;
			}
		}

		/// <summary>
		/// Starts the activity.
		/// </summary>
		/// <param name="scope">The activity scope.</param>
		public static void Start(ActivityScope scope)
		{
			ActivityStack.Push(scope);
		}

		/// <summary>
		/// Ends the activity.
		/// </summary>
		/// <param name="scope">The activity scope.</param>
		public static void End(ActivityScope scope)
		{
			if (Current == null)
				return;

			if (!ActivityStack.Any(scopeOnTheStack => scope.Id == scopeOnTheStack.Id))
				return;

			ActivityScope currentScope = ActivityStack.Pop();
			while (ActivityStack.Count > 0 && currentScope.Id != scope.Id)
				currentScope = ActivityStack.Pop();
		}
	}
}
#endif
