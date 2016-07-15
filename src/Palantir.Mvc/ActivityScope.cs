/// <acknowledgements>
/// Adapted from https://github.com/CorrelatorSharp/CorrelatorSharp since it didn't support CoreCLR.
/// </acknowledgements>

#if DNX46
namespace Palantir.Mvc
{
	using System;
	using System.Diagnostics.Contracts;

	public sealed class ActivityScope : IDisposable
	{
		private readonly string id;
		private readonly string parentId;
		private readonly string name;

		/// <summary>
		/// Creates the activity scope.
		/// </summary>
		/// <param name="name">The activity scope name.</param>
		/// <param name="id">The activity scope ID.</param>
		/// <param name="parentId">The activity scope parent ID.</param>
		public ActivityScope(string name, string id = null)
		{
			Contract.Requires(!string.IsNullOrEmpty(name));

			this.name = name;
			this.id = id ?? Guid.NewGuid().ToString("D");
			this.parentId = ActivityTracker.Current?.Id;

			ActivityTracker.Start(this);
		}

		/// <summary>
		/// The current activity scope.
		/// </summary>
		public static ActivityScope Current => ActivityTracker.Current;

		/// <summary>
		/// The activity scope ID.
		/// </summary>
		public string Id => id;

		/// <summary>
		/// The parent scope ID.
		/// </summary>
		public string ParentId => parentId;

		/// <summary>
		/// The name for the activity.
		/// </summary>
		public string Name => name;

		/// <summary>
		/// Disposes the activity.
		/// </summary>
		public void Dispose()
		{
			ActivityTracker.End(this);
		}
	}

}
#endif
