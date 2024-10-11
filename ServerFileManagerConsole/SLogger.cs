using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace ServerFileManagerConsole;

public class SLogger
{
    static readonly Logger Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();
    
    public static void Log(string message)
    {
        Logger.Information(message);
        Console.ResetColor();

    }
    
    public static void LogError(string message)
    {
        Logger.Error(message);
        Console.ResetColor();
        Console.ReadLine();
    }
}