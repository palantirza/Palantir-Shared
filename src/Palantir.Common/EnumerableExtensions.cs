namespace Palantir
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// IEnumerable extension methods.
	/// </summary>
	public static class EnumerableExtensions
    {
		/// <summary>
		/// If <paramref name="source"/> is null, return an empty enumerable, otherwise return <paramref name="source"/>.
		/// </summary>
		/// <typeparam name="TSource">The source type.</typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public static IEnumerable<TSource> NullToEmpty<TSource>(this IEnumerable<TSource> source)
		{
			Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

			if (source == null)
				return Enumerable.Empty<TSource>();

			return source;
		}

		// From http://stackoverflow.com/questions/2471588/how-to-get-index-using-linq
		///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
		///<param name="items">The enumerable to search.</param>
		///<param name="predicate">The expression to test the items against.</param>
		///<returns>The index of the first matching item, or -1 if no items match.</returns>
		public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
		{
			if (items == null) throw new ArgumentNullException("items");
			if (predicate == null) throw new ArgumentNullException("predicate");

			int retVal = 0;
			foreach (var item in items)
			{
				if (predicate(item)) return retVal;
				retVal++;
			}
			return -1;
		}

		// From http://stackoverflow.com/questions/2471588/how-to-get-index-using-linq
		///<summary>Finds the index of the first occurence of an item in an enumerable.</summary>
		///<param name="items">The enumerable to search.</param>
		///<param name="item">The item to find.</param>
		///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
		public static int IndexOf<T>(this IEnumerable<T> items, T item)
		{
			return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i));
		}

		// From http://stackoverflow.com/questions/2471588/how-to-get-index-using-linq
		///<summary>Fetches the item at the specified index in an enumerable.</summary>
		///<param name="items">The enumerable to search.</param>
		///<param name="index">The index to fetch.</param>
		///<returns>The item.</returns>
		public static T Index<T>(this IEnumerable<T> items, int index)
		{
			using (var enumerator = items.GetEnumerator())
			{
				enumerator.Reset();
				for (int i = 0; i <= index; i++)
					enumerator.MoveNext();

				return enumerator.Current;
			}
		}
	}
}
