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

