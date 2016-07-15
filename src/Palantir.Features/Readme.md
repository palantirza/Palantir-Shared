# Palantir.Features
**Requires**

* Serilog

A Feature Toggle implementation that provides full Dependendency Injection and follows Martin Fowler's advice on Feature Toggle
design. It provides a configurable mechanism to supply features, and allows different feature sets to come from different
feature sources.

First, you need to declare your Features, which is can be an interface or a POCO, with a set of boolean Properties,
each one representing a feature:
```
public class MyFeatures
{
    public bool FeatureA { get; }
    public bool FeatureB { get; }
}
```

You don't need setters, since the class will never be instantiated directly.

## AppSettings Configuration

Then, you'll need to wire it up. For this example, we'll use the OptionsModelFeatureConfiguration, which gets the features settings
via an IOptions interface. The IOptions will be provided via the appsettings.json.

Add the following code to ConfigureServices:
```
services.ConfigureFeatures<MyFeatures>(Configuration.GetSection("MyFeatures"));
```

This line configures the features. Now you need to add the feature settings to the appsettings.json:
```
{
    "MyFeatures": {
        "Features": [
            {
                "Name": "MyFeatures.FeatureA",
                "Rules": [
                    { "Type": "enabled", "Enabled": true }
                ]
            }
        ]
    }
}
```

Any feature not configured, or with no rules, will be disabled by default.

## Dependency Injection

Now, all you need to do is to require the strongly typed IFeatureRouter in your controller:

```
public class MyController : Controller
{
    public MyController(IFeatureRouter<MyFeatures> features) 
    {
    }
}
```

And to use it is as simple as anything:
```
if (features.IsFeatureEnabled(x => x.FeatureA))
{
    // Do cool feature stuff
}
```

## Feature Decisions
You can also ask for a consolidated list of the feature decisions in one operation:

```
MyFeatures featureDecisions = features.GetFeatureDecisions();
```

This will create a standin for the Features and ensure that the relevant features are returned. If your Features class is a POCO,
your MUST ensure that the Features (public boolean properties) are settable, or this will error.

You can also ask for the Feature Decisions to be injected directly into your class:

```
public class MyController : Controller
{
    public MyController(MyFeatures featureDecisions) 
    {
    }
}
```

**NB**: Features must be settable in order to use the decisions functionality. The system needs to be able to set the properties.

## Customise feature names

You can give the feature a specific name, which can be used in IsFeatureEnabled and the appsettings.json file by adding
a FeatureName attribute to it:

```
public class MyFeatures
{
    [FeatureName("cool-feature")]
    public bool FeatureA { get; }
    public bool FeatureB { get; }
}
```

A feature name replaces the **entire** name:

```
{
    "MyFeatures": {
        "Features": [
            {
                "Name": "cool-feature",
                "Rules": [
                    { "Type": "enabled", "Enabled": true }
                ]
            }
        ]
    }
}
...
if (features.IsFeatureEnabled("cool-feature"))
...
```

You can still use the usual expression syntax, even with a named feature:

```
...
if (features.IsFeatureEnabled(x => x.FeatureA))
...
```

## Expiring toggles
For strongly typed toggles, we suggest adding an Obsolete attribute to the toggle property:
```
public class MyFeatures
{
    [Obsolete("Remove after release", false)]
    public bool FeatureA { get; set; }
    public bool FeatureB { get; set; }
}
```

This will give a warning where that feature is used in the code (when using strongly typed expressions to identify features).
Changing the error property to true will break the build until all the feature calls have been removed.

But what about feature requests that use the string syntax:
```
if (features.IsFeatureEnabled("MyFeatures.FeatureA"))
```

The features system will log a WARNING entry about the obsolete toggle when it is still accessed. If it has an error of true,
it will throw an ObsoleteFeatureException and log an ERROR entry about the Deprecated feature.

## Other Feature Rules

**ROADMAP**

* EnabledForVariable - Feature is enabled for a specific context variable, e.g. host or username
* EnabledAfter - Feature is enabled after a given time
* EnabledBefore - Feature is enabled before a given time
* EnabledOnBreaker - Feature is enabled if a circuit breaker is set

## Other Feature Sources

**ROADMAP**

* Database/NoSQL
* Consul/Zookeeper