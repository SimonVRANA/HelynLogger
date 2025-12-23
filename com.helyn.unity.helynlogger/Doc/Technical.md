
# Technical Documentation

## Class list
- **LoggerSetup.cs**: The package entry point. Creates loggers and keeps track of them.
- **LoggerSettings.cs**: All settings for the loggers (both console and file).
- **LoggerSettingsLoader.cs**: Loads and saves settings from a JSON file in `Application.streamingAssetsPath`.

- UnityLogger/**UnityLoggerProvider.cs**: Provides a UnityLogger for each category (namespace + class name).
- UnityLogger/**UnityLogger.cs**: Logs messages to the Unity console.
- UnityLogger/**UnityLogFormatter.cs**: Formats log messages using the format provided in the *Unity console* section of the `LoggerSettings`.

- SimpleFileLoader/**SimpleFileLoaderProvider.cs**: Provides a SimpleFileLoader for each category (namespace + class name).
- SimpleFileLoader/**SimpleFileLoader.cs**: Logs messages to a file.
- SimpleFileLoader/**SimpleFileLogFormatter.cs**: Formats log messages using the format provided in the *File* section of the `LoggerSettings`.

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

    LoggerSetup ->> LoggerSetup: Read `IConfiguration` from the file specified in the settings
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
The Unity console logger and the file logger work the same way, except for the Log method.

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

    opt is enabled
    participant UnityLogFormatter
    UnityLogger ->>+ UnityLogFormatter: FormatLogMessage
    UnityLogFormatter -->>- UnityLogger:

    participant UnityEngine.Debug
    UnityLogger ->> UnityEngine.Debug: Log/LogWarning/LogError/LogException
    deactivate UnityLogger
    end

```

### File log
The Unity console logger and the file logger work the same way, except for the Log method.
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

    opt is enabled
    participant SimpleFileLogFormatter
    SimpleFileLogger ->>+ SimpleFileLogFormatter: FormatLogMessage
    SimpleFileLogFormatter -->>- SimpleFileLogger:

    participant System.IO.File
    SimpleFileLogger ->> System.IO.File: AppendAllText
    deactivate SimpleFileLogger
    end

```