
# Technical documentation

## Class list
- **LoggerSetup.cs**: The package entry point, creates loggers and keeps track of them.
- **LoggerSettings.cs**: All settings for the loggers (both console and file).
- **LoggerSettingsLoader.cs**: Loads and save the settings from a Json file in (Application.streamingAssetsPath).
- **LoggerFilder.cs**: Provide a way to filter logs with default C# class base filters (More info in the [LoggerFilter section](#loggerfiltercs---a-workaround)).
<br><br>
- UnityLogger/**UnityLoggerProvider.cs**: Provides a UnityLogger for each category (namespace+classname).
- UnityLogger/**UnityLogger.cs**: Logs the messages to the Unity console.
- UnityLogger/**UnityLogFormatter.cs**: Formats the log using the format provided in the *Unity console* section of the LoggerSettings.
<br><br>
- SimpleFileLoader/**SimpleFileLoaderProvider**: Provides a SimpleFileLoader for each category (namespace+classname).
- SimpleFileLoader/**SimpleFileLoader.cs**: Logs the messages to a file.
- SimpleFileLoader/**SimpleFileLogFormatter.cs**: Formats the log using the format provided in the *File* section of the LoggerSettings.

## Flow
### Start

```mermaid
sequenceDiagram
    participant LoggerSetup
    
    participant LoggerSettingsLoader
    LoggerSetup ->>+ LoggerSettingsLoader: LoadSettings
    activate LoggerSettingsLoader
    LoggerSettingsLoader -->> LoggerSettingsLoader: Load settings from Application.streamingAssetsPath
    deactivate LoggerSettingsLoader
    LoggerSettingsLoader -->>- LoggerSetup: return settings

    LoggerSetup ->> LoggerSetup: Read IConfiguration from file set in settings
    LoggerSetup ->>+ LoggerSetup: Create logger factory
    LoggerSetup ->> LoggerSetup: Add configuration and debug provider
    opt Unity logs enabled in settings
    LoggerSetup ->> LoggerSetup: Add UnityLoggerProvider
    end
    opt File logs enabled in settings
    LoggerSetup ->> LoggerSetup: Add SimpleFileLoggerProvider
    end
    LoggerSetup -->>- LoggerSetup: 

```

### Unity console log
Unity console logger and File logger works the same, except for the Log method.

```mermaid
sequenceDiagram
    participant AClass
    participant LoggerSetup

    AClass ->>+ LoggerSetup: LoggerFactory.CreateLogger
    participant UnityLoggerProvider
    LoggerSetup ->>+ UnityLoggerProvider: CreateLogger
    UnityLoggerProvider -->>- LoggerSetup:
    LoggerSetup -->>- AClass:

    participant UnityLogger
    AClass ->>+ UnityLogger: Log

    participant LoggerFilter
    UnityLogger ->>+ LoggerFilter: IsEnabled
    LoggerFilter ->> LoggerFilter: Create "dummy" logger
    LoggerFilter -->>- UnityLogger:

    opt is enabled
    participant UnityLogFormatter
    UnityLogger ->>+ UnityLogFormatter: FormatMessageFormatLogMessage
    UnityLogFormatter -->>- UnityLogger:

    participant UnityEngine.Debug
    UnityLogger ->> UnityEngine.Debug: Log/LogWarning/LogError/LogException
    deactivate UnityLogger
    end

```

### File log
Unity console logger and File logger works the same, except for the Log method.
```mermaid
sequenceDiagram
    participant AClass
    participant LoggerSetup

    AClass ->>+ LoggerSetup: LoggerFactory.CreateLogger
    participant SimpleFileLoggerProvider
    LoggerSetup ->>+ SimpleFileLoggerProvider: CreateLogger
    SimpleFileLoggerProvider -->>- LoggerSetup:
    LoggerSetup -->>- AClass:

    participant SimpleFileLogger
    AClass ->>+ SimpleFileLogger: Log

    participant LoggerFilter
    SimpleFileLogger ->>+ LoggerFilter: IsEnabled
    LoggerFilter ->> LoggerFilter: Create "dummy" logger
    LoggerFilter -->>- SimpleFileLogger:

    opt is enabled
    participant SimpleFileLogFormatter
    SimpleFileLogger ->>+ SimpleFileLogFormatter: FormatMessageFormatLogMessage
    SimpleFileLogFormatter -->>- SimpleFileLogger:

    participant System.IO.File
    SimpleFileLogger ->> System.IO.File: AppendAllText
    deactivate SimpleFileLogger
    end

```

## LoggerFilter.cs - a workaround
I did not find a way to get the default category filtering in my custom loggers. So the class is a workaround to get access to this filtering "outside" of my custom loggers.

The LoggerFilter class creates a "default" LoggerFactory that only create a configuration. Loggers are stored in a dictionary for easier access.

My loggers IsEnabled method simply calls the LoggerFilter. LoggerFilter will then get or create the "fake default" logger and call IsEnabled on it to tell my logger if it should be enabled or not for this category.