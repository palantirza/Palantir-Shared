namespace Palantir.Paging
{
    using PagedList;
    using Raven.Client;
    using Raven.Client.Linq;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public static class QueryableExtensions
    {
        /// <summary>
        /// Converts the queryable to a paged list.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="source">The source queryable.</param>
        /// <param name="page">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The paged list.</returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IRavenQueryable<T> source, int page, int pageSize)
        {
            var totalItemCountTask = source.CountAsync();

            var items = source.Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();

            await Task.WhenAll(items, totalItemCountTask);

            return new SerializablePagedList<T>(items.Result, page, pageSize, (int)totalItemCountTask.Result);
        }

        /// <summary>
        /// Converts the queryable to a paged list.
        /// </summary>
        /// <typeparam name="T">The type of item.</typeparam>
        /// <param name="source">The source queryable.</param>
        /// <param name="page">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The paged list.</returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int page, int pageSize)
        {
            var ravenQueryable = source as IRavenQueryable<T>;
            if (ravenQueryable != null)
                return await ToPagedListAsync(ravenQueryable, page, pageSize);

            throw new NotSupportedException(SR.Err_PagedListAsyncRequiresRaven());
        }
    }
}
