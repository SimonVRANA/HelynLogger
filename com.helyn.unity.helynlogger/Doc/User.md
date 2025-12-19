# Helyn logger — Prototype - User manual

As this is an abandoned project, it comes with no warranty to work.

## Install
To install this package in Unity, //TODO see how to do that in designsystem package

## Usage
To log, you'll first need to create a logger dedicated to your class:
```cs
private static readonly ILogger helynLogger = LoggerSetup.LoggerFactory.CreateLogger();
```

Then you can log with the same method provided by default C# ILogger:
```cs
helynLogger.
```






TODO: 
- open in Unity
- make an example folder
- create an example class
- fix configuration loading (bool "from StreamingAsset/absolute" path for ConfigBasePath in settings)
- finish previous section (do not forget that default settings can be generated if logger was created once) 



## Customization
Since Configuration is already part of C# default vocabulary, in this documentation we'll refer to the following words as:
- Settings: the settings for this package.
- Configuration: same as in C# default logging library: the filters to disable logs based on level and category (namespace + class name).

### Settings
TODO: Explain settings

### Configuration
Same as in C# default logging library: the filters to disable logs based on level and category (namespace + class name).

Configuration is loaded from a json file, as set in the app settings. 