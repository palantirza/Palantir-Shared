Health Checks
=============

In a microservice world, it becomes critical to get up to date detailed information for how the microservices are performing. Many
systems provide health checks, end points at which this information can be gleaned. 

The Palantir system of Health Checks works off the concept of named Health Monitoring Policies. A Policy consists of a set of
Indicators which report on a value, and whether it is within allowed values. Every Indicator has three allowed outcomes:

* **OK**: Everything is fine
* **Warning**: A problem has been detected, which could become a problem
* **Error**: There is a severe issue which is either causing an outage, or will soon cause an outage

The overall policy is evaluated to provide an aggregated status, which is simply the worst status of the indicators in the policy.

Health repots for all policies can be viewed in a JSON format by navigating to the ```\health``` endpoint. In addition, every
call to a service which has the ```HealthCheck``` attribute defined will report the overall status of the policy in the
```X-Health-Check``` response header.

Using Health Check Monitoring
-----------------------------

** Startup.cs Configure Service**

```c#
public override void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddHealthMonitoring();
}
```

** Startup.cs Configure Service**

```c#
public override void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddHealthMonitoring();
}
```

This information can include circuit breaker and feature toggle states.

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

Roadmap
-------

* Health checks must be performed as background tasks and reported on quickly
* RavenDB check
* NServiceBus check
* CPU & Memory usage check