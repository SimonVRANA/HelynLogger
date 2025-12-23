# Helyn Logger — Prototype — User Manual

This project is provided as-is, with no warranty that it will work.

## Install
To install this package in Unity, you have two options:
- Add `"com.helyn.unity.helynlogger": "https://github.com/SimonVRANA/HelynLoggerForCSharp.git?path=com.helyn.unity.helynlogger"` to your `Packages/manifest.json` file.
- Go to Window > Package Manager > v + (top-left) > **Add package from git URL...** and paste: `https://github.com/SimonVRANA/HelynLoggerForCSharp.git?path=com.helyn.unity.helynlogger`.

## Usage
To log, create a logger for your class:
```cs
private static readonly ILogger helynLogger = LoggerSetup.LoggerFactory.CreateLogger<MyClass>();
```

Then you can use the same methods provided by the default C# `ILogger`:
```cs
helynLogger.LogTrace("Trace message");
helynLogger.LogDebug("Debug message");
helynLogger.LogInformation("Example message");
helynLogger.LogWarning("Low disk space: {FreeMb} MB", freeMb);
helynLogger.LogError(ex, "Failed to process request {RequestId}", requestId);
helynLogger.LogCritical("Critical failure: {Error}", error);
```

Customization files are automatically created upon installation. If they are not created, run a scene or restart Unity.

## Examples
Examples are available in the `Examples/` folder.


## Customization
Because 'Configuration' is already a term in the default C# libraries, this documentation uses the following definitions:
- Settings: the settings for this package.
- Configuration: same as in C# default logging library: the filters to disable logs based on level and category (namespace + class name).

File location:
- The settings file location is hard-coded in `LoggerSettingsLoader` to `[StreamingAssets]/HelynLogger/HelynLoggerSettings.json`.
- The configuration file location is set in the settings file.

### Settings
#### Common
- **ConfigFolderPath**: the folder containing the configuration file. If the path isn't rooted, it will be prefixed with `Application.streamingAssetsPath`.
- **ConfigFileName**: the name of the configuration file.

#### Unity
- **EnableConsoleLogging**: Enable or disable Unity console logging.
- **ConsoleLogFormat**: Console log format. Supported substitution keywords:
    - `{timestamp:[Format]}`: the log timestamp. Supported formats are the same as [DateTime.ToString()](https://learn.microsoft.com/dotnet/api/system.datetime.tostring?view=net-8.0).
    - `{level}`: the log level.
    - `{category}`: the log category (namespace + class name).
    - `{message}`: the log message.
- **ColorLogLevel**: Enable or disable log level coloring.
- **Level Colors**: Define the color for each log level. Supported colors use Unity's `<color>` tag (probably [Rich Text](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html)).
    - TraceColor
    - DebugColor
    - InformationColor
    - WarningColor
    - ErrorColor
    - CriticalColor
    - NoneColor (default)

#### File
- **EnableFileLogging**: Enable or disable file logging.
- **LogFilePath**: The path of the log file. If the path is not rooted, it will be prefixed with `Application.streamingAssetsPath`.
- **FileLogFormat**: File log format. Substitution keywords are the same as the Unity console (see `Unity` above).

### Configuration
Same as in the default C# logging library: filters that control which logs are enabled based on level and category (namespace + class name).

The configuration is loaded from a JSON file specified in the application settings. 