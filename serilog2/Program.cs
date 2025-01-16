using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Configure Serilog to log to multiple sinks: Console, File, and Rolling File
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithProperty("Application", "SerilogDemo") // Add a custom property
            .Enrich.FromLogContext() // Enrich with additional context like method names
            .WriteTo.Console(
                outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}") // Console format
            .WriteTo.File(
                @"C:\Users\amrut\Desktop\AMRUTA\repo\serilog2\log.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}") // File format
            .WriteTo.File(
                @"C:\Users\amrut\Desktop\AMRUTA\repo\serilog2\log_all.txt",
                rollingInterval: RollingInterval.Hour,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug) // Log file for all logs (with Debug level and above)
            .CreateLogger();

        // Example of logging an event
        Log.Information("Application Starting");

        // Add a custom property in logs
        Log.Information("User {UserName} has started the application.", Environment.UserName);

        // Performance Logging Example: Measure how long an operation takes
        Stopwatch stopwatch = Stopwatch.StartNew();
        await SimulateLongRunningOperation();
        stopwatch.Stop();
        Log.Information("Operation took {ElapsedMilliseconds} ms.", stopwatch.ElapsedMilliseconds);

        // Conditional Logging: Log different levels based on conditions
        int value = new Random().Next(1, 100);
        if (value > 75)
        {
            Log.Warning("Value {Value} is greater than threshold.", value);
        }
        else
        {
            Log.Debug("Value {Value} is within acceptable range.", value);
        }

        // Example of logging an error with exception
        try
        {
            // Your application logic
            throw new InvalidOperationException("Example exception!");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred");
        }

        // Close and flush the log
        Log.CloseAndFlush();
    }

    // Simulate a long-running operation (like a database query, file operation, etc.)
    static async Task SimulateLongRunningOperation()
    {
        await Task.Delay(2000); // Simulate a delay of 2 seconds
    }
}
