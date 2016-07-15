namespace Palantir.EventStore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

    public interface IEventStreamInfo
    {
		Guid StreamId { get; }
		int StreamRevision { get; }
		int EventCount { get; }
		DateTimeOffset DateCreatedUtc { get; }
		DateTimeOffset LastModifiedDateUtc { get; }
    }
}
