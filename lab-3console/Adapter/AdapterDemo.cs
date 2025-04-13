using System;
using System.IO;

public interface ILogger
{
    void Log(string message);
    void Error(string message);
    void Warn(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[LOG] " + message);
        Console.ResetColor();
    }

    public void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ERROR] " + message);
        Console.ResetColor();
    }

    public void Warn(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[WARN] " + message);
        Console.ResetColor();
    }
}

public class FileWriter
{
    private string filePath;
    public FileWriter(string filePath)
    {
        this.filePath = filePath;
    }

    public void Write(string text)
    {
        File.AppendAllText(filePath, text);
    }

    public void WriteLine(string text)
    {
        File.AppendAllText(filePath, text + Environment.NewLine);
    }
}

public class FileLoggerAdapter : ILogger
{
    private FileWriter fileWriter;

    public FileLoggerAdapter(string filePath)
    {
        fileWriter = new FileWriter(filePath);
    }

    public void Log(string message)
    {
        fileWriter.WriteLine("[LOG] " + message);
    }

    public void Error(string message)
    {
        fileWriter.WriteLine("[ERROR] " + message);
    }

    public void Warn(string message)
    {
        fileWriter.WriteLine("[WARN] " + message);
    }
}

public class AdapterDemo
{
    public static void RunDemo()
    {
        Console.WriteLine("=== Adapter Demo ===");

        ILogger consoleLogger = new ConsoleLogger();
        consoleLogger.Log("Console logger log message");
        consoleLogger.Error("Console logger error message");
        consoleLogger.Warn("Console logger warning message");

        string filePath = "logfile.txt";
        ILogger fileLogger = new FileLoggerAdapter(filePath);

        fileLogger.Log("File logger log message");
        fileLogger.Error("File logger error message");
        fileLogger.Warn("File logger warning message");

        Console.WriteLine("Перевірте файл logfile.txt, щоб побачити збережені логи.");
    }
}