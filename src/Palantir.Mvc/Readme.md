Circuit Breakers
================

Circuit breakers are conceptually similar to the breakers you'll find in your home's distribution box. They are used to 
isolate systems that are problematic, as well as return error information quickly to the user. They also act as a cue as to
what is wrong with the system, just as home breakers do, you simply look at what tripped. They can be in three positions:

* **Open** - The system isolated by the breaker 
* **Closed** - The breaker allows all calls through
* **Half Open** - The breaker allows the next call through, if it fails it becomes Open if it succeeds it becomes Closed

Circuit breakers are normally used for remote call invocations, where errors can take a long time to return to the client, and
a stampede of requests could interfere with recovery options. When the breaker trips, clients receive errors immediately, and
the target system is isolated from the calls.

Circuit breakers can easily be used in conjunction with health checks, using the checks to determine when to trip, or when to
move from open to to half open.

Advanced Possibilities
----------------------
Circuit breakers can be holding up a flood of requests, and when they're reset the requests can overwhelm the target machine.
They could be configured to slowly ramp up the calls to the target without putting it under undue load.

Error Handling
==============

Exceptions should be limited to truly exceptional situations. **Never** for user input or other validations. When you want to
return a Failed result, return a value for it.

For example, in Controllers, the return type should **always** be IActionResult, and non serious "errors" should return failure
result types.

Features
========

A feature is a set of functionality which is provided to users. Features are used for the following reasons:

* **Billing/Usage** - The usage of the feature needs to be tracked for billing or statistics purposes.
* **Segmentation** - Access to the feature needs to be controlled based on client or product segment.
* **Release** - The feature is being introduced and needs to be selectively enabled/disabled in order to support
a staged, and reversible rollout.

Each feature has a name, and, it is strongly suggested, a version.

Health Checks
=============

In a microservice world, it becomes critical to get up to date detailed information for how the microservices are performing. Many
systems provide health checks, end points at which this information can be gleaned. Generally it consists of an UP/DOWN status
along with more detailed information.

This information can include circuit breaker and feature toggle states.

Service Levels
==============

Service Levels describe acceptable operating parameters for a system. Service Levels normally define either hard limits,
or statistical measures

```
Call Duration must be less than 100ms
Call Duration must be less than 100ms σ1
```

Which means it must be within one standard deviation (or sigma) - 68.27% of 100ms, or 

```
Call Duration must be less than 100ms σ0 @ 99%, σ0 at 100%
```

Which means it must be less than 100ms 99% of the time, and within one standard deviation the rest of the time.

Ideally Service Levels should be defined including the expected load. It's often unreasonable to expect something to perform the same
for 10 users as for 10,000.

Configuration
=============

Configuration entries are defined between two axes: Consistency and Distribution.

Consistency
-----------

Why the configuration entry changes:

1. *Operational* - the configuration changes for software system reasons, e.g. timeout values
2. *Business* - the configuration changes for business operational reasons, e.g. risk limits

Configuration that never really changes should be hardcoded.

Distribution
------------

How widely the configuration setting applies:

1. *Environment* - all within a given environment use the same value.
2. *Host* - all within a given host use the same value.

Configuration that changes for other reasons should be coded into business rules.

Sources
-------

The following configuration sources are specified:

1. *File* - the configuration is specified in a file, local to the system, e.g. appsettings.json.
2. *Variable* - the configuration is specified in an environmental variable, local to the host, e.g. NUMBER_OF_PROCESSORS.
3. *Store* - the configuration is specified in a database store, local to the system, e.g. RavenDB.
4. *Registry* - the configuration is specified in a distributed system registry, e.g. Consul or Zookeeper.

Mappings
--------

Below are the mappings of configuration types to source:

* **Operational/Environment** - Registry
* **Operations/Host** - File/Variable
* **Business/Environment** - Store
* **Business/Host** - Not Applicable

The choice between File or Variable is a simple one: if it is host, or container wide it should be in an environmental variable,
if it is specific to the app, it should be in the configuration file.