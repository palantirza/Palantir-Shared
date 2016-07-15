## Enumerable Extensions

### NullToEmpty
If the IEnumerable is null, it will be converted to an Empty enumerable.

```
var x = source.NullToEmpty(); // If source is null, x will be an empty Enumerable
```

## Logging Extensions

Extensions to Serilog. For each logging level we add extensions with the suffix ...Event, which take and `eventId` and an optional 
`transactionId`. The `eventId` is supposed to  be unique to each log entry location, and is used to tie the log back to the code.
The `transactionId` is used to provide a unique instance for the call or caller, to allow the transaction to be tracked across 
multiple locations.

### With

Allows you to add a property value to every call after that point.

```
var log2 = log.With("MyProp", 12);
log2.InformationEvent("MyEvent", "Test log for {Name}", "My name"); // Will include MyProp = 12 in the log values.
```

## Immutable Attribute

Used to indicate that a given class is immutable. Used by tools to verify immutability operations.

`IsStrict` should only be true if every field is readonly.

## MultiValueDictionary

Microsoft code, not yet in System.Collections.Generic. Will be deprecated and replaced with the Microsoft version when it goes live.

## Paging

SerializablePagedList to allow a PagedList to be returned from WebApi methods.

## Messaging

An in memory event bus implementation. Based off some NServiceBus concepts but with full async support and cancellation tokens.

## EventStore

An event store implementation based off similar concepts as NEventStore but using NServicenus style events.

### Roadmap

* RavenDb, MongoDB implementations for EventStore
* MSMQ and other queue implementations for Messaging