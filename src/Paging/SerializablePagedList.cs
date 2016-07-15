namespace Palantir.Paging
{
	using Newtonsoft.Json;
	using PagedList;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// A PagedList that can be serialized to JSON.
	/// </summary>
	/// <typeparam name="T">The PagedList type.</typeparam>
	[JsonConverter(typeof(SerializablePagedListConverter))]
	public class SerializablePagedList<T> : BasePagedList<T>
	{
		/// <summary>
		/// Initializes a new instance of the Serializable class.
		/// </summary>
		public SerializablePagedList()
		{
		}

		/// <summary>
		/// Initializes a new instance of the Serializable class.
		/// </summary>
		/// <param name="items">The items to initialize with.</param>
		public SerializablePagedList(IPagedList<T> items)
			: base(items.PageNumber, items.PageSize, items.TotalItemCount)
		{
			Subset.AddRange(items.Skip(items.PageNumber - 1 * items.PageSize).Take(items.PageSize));
		}

		/// <summary>
		/// Initializes a new instance of the Serializable class.
		/// </summary>
		/// <param name="items">The subset items.</param>
		/// <param name="page">The current page.</param>
		/// <param name="pageSize">The page size.</param>
		/// <param name="totalItemCount">The superset size.</param>
		public SerializablePagedList(IEnumerable<T> items, int page, int pageSize, int totalItemCount)
			: base(page, pageSize, totalItemCount)
		{
			Subset.AddRange(items);
		}

	}
}
