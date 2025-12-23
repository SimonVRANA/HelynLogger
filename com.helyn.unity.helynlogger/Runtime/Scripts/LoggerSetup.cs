// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Helyn.Logger
{
	public static class LoggerSetup
	{
		private static readonly object lockObject = new();
		private static bool isInitialized = false;
		private static ILoggerFactory loggerFactory;
		private static IConfiguration configuration;
		private static LoggerSettings settings;

		private static ILoggerFactory filterFactory;
		private static readonly ConcurrentDictionary<string, Microsoft.Extensions.Logging.ILogger> filterLoggers = new();

		public static ILoggerFactory LoggerFactory
		{
			get
			{
				Initialize();
				return loggerFactory;
			}
		}

		public static IConfiguration Configuration
		{
			get
			{
				Initialize();
				return configuration;
			}
		}

		internal static LoggerSettings Settings
		{
			get
			{
				Initialize();
				return settings;
			}
		}

		public static void Initialize()
		{
			if (isInitialized)
			{
				return;
			}

			settings = LoggerSettingsLoader.LoadSettings();

			lock (lockObject)
			{
				CreateConfiguration();
				CreateLoggerFactory();

				isInitialized = true;
			}
		}

		private static void CreateConfiguration()
		{
			EnsureConfigFileExists();

			ConfigurationBuilder configurationBuilder = new();
			configurationBuilder.SetBasePath(settings.ConfigFilePath);
			configurationBuilder.AddJsonFile(settings.ConfigFileName);

			configuration = configurationBuilder.Build();
		}

		private static void EnsureConfigFileExists()
		{
			string configFilePath = System.IO.Path.Combine(settings.ConfigFilePath, settings.ConfigFileName);
			if (!System.IO.File.Exists(configFilePath))
			{
				var defaultConfig = new
				{
					Logging = new
					{
						LogLevel = new
						{
							Default = "Information",
							Microsoft = "Warning",
							System = "Warning"
						}
					}
				};
				string json = JsonConvert.SerializeObject(defaultConfig, Formatting.Indented);

				if (!System.IO.Directory.Exists(settings.ConfigFilePath))
				{
					System.IO.Directory.CreateDirectory(settings.ConfigFilePath);
				}

				System.IO.File.WriteAllText(configFilePath, json);
			}
		}

		private static void CreateLoggerFactory()
		{
			loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(ConfigureLoggingFactoryBuilder);
		}

		private static void ConfigureLoggingFactoryBuilder(ILoggingBuilder builder)
		{
			builder.AddConfiguration(configuration.GetSection("Logging"));
			// Debug provider
			builder.AddDebug();

			// Unity output
			if (settings.EnableConsoleLogging)
			{
				builder.AddProvider(new UnityLoggerProvider());
			}
			// File output
			if (settings.EnableFileLogging)
			{
				builder.AddProvider(new SimpleFileLoggerProvider());
			}
		}

		public static void Dispose()
		{
			lock (lockObject)
			{
				if (!isInitialized)
				{
					return;
				}

				loggerFactory?.Dispose();
				loggerFactory = null;
				configuration = null;
				isInitialized = false;
			}
		}
	}
}