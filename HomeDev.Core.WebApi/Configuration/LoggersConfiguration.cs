using System;
using HomeDev.Core.WebApi.Interfaces;
using Serilog;
using Serilog.Events;

namespace HomeDev.Core.WebApi.Configuration
{
    public static class LoggersConfiguration
    {
        public static void SetupLoggers(IApiSettings apiSettings) 
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("ServerName", Environment.MachineName)
                .Enrich.WithProperty("Environment", apiSettings.Environment)
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.WithProcessId()
                .Enrich.FromLogContext();

            SetupConsoleLogger(loggerConfiguration);
            SetupSeqLogger(loggerConfiguration, apiSettings);

            Log.Logger = loggerConfiguration.CreateLogger();
        }

        private static void SetupConsoleLogger(LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration.WriteTo.Console(LogEventLevel.Debug);
        }

        private static void SetupSeqLogger(LoggerConfiguration loggerConfiguration, IApiSettings apiSettings)
        {
            loggerConfiguration.WriteTo.Seq(apiSettings.SeqUrl);
        }
    }
}