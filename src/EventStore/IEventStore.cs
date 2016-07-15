namespace Palantir.EventStore
{
	using PagedList;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public interface IEventStore
    {
		Task<IEventStream> OpenStream(Guid streamId, int minVersion = int.MinValue, int maxVersion = int.MaxValue);

		Task<PagedList<IEventStreamInfo>> Find(Func<IEventStreamInfo, bool> predicate, int page = 1, int pageSize = 50);
	}
}
