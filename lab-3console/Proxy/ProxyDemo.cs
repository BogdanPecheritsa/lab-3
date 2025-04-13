using System;
using System.IO;
using System.Text.RegularExpressions;

public interface ITextReader
{
    char[][] ReadFile(string path);
    void Close();
}

public class SmartTextReader : ITextReader
{
    private bool fileOpened = false;
    private StreamReader reader;
    private char[][] content;

    public char[][] ReadFile(string path)
    {
        reader = new StreamReader(path);
        fileOpened = true;

        var lines = new System.Collections.Generic.List<char[]>();
        string line;

        while ((line = reader.ReadLine()) != null)
        {
            lines.Add(line.ToCharArray());
        }

        content = lines.ToArray();
        return content;
    }

    public void Close()
    {
        if (fileOpened && reader != null)
        {
            reader.Close();
            fileOpened = false;
        }
    }
}

public class SmartTextChecker : ITextReader
{
    private SmartTextReader realReader;
    private bool isFileOpened = false;
    private string currentFile;

    public char[][] ReadFile(string path)
    {
        Console.WriteLine($"[SmartTextChecker] Opening file: {path}");
        realReader = new SmartTextReader();
        var result = realReader.ReadFile(path);
        isFileOpened = true;
        currentFile = path;

        int totalLines = result.Length;
        int totalChars = 0;
        foreach (var line in result)
        {
            totalChars += line.Length;
        }

        Console.WriteLine($"[SmartTextChecker] File read successfully. Lines: {totalLines}, Characters: {totalChars}");
        return result;
    }

    public void Close()
    {
        if (isFileOpened)
        {
            realReader.Close();
            Console.WriteLine($"[SmartTextChecker] File {currentFile} is closed.");
            isFileOpened = false;
        }
    }
}

public class SmartTextReaderLocker : ITextReader
{
    private SmartTextReader realReader;
    private Regex deniedPattern;
    private bool isFileOpened = false;

    public SmartTextReaderLocker(string pattern)
    {
        deniedPattern = new Regex(pattern, RegexOptions.IgnoreCase);
    }

    public char[][] ReadFile(string path)
    {
        if (deniedPattern.IsMatch(path))
        {
            Console.WriteLine($"[SmartTextReaderLocker] Access denied for file: {path}");
            return null;
        }
        else
        {
            realReader = new SmartTextReader();
            var result = realReader.ReadFile(path);
            isFileOpened = true;
            return result;
        }
    }

    public void Close()
    {
        if (isFileOpened && realReader != null)
        {
            realReader.Close();
            isFileOpened = false;
        }
    }
}

public class ProxyDemo
{
    public static void RunDemo()
    {
        Console.WriteLine("=== Proxy Demo ===");

        string sampleFile = "sample.txt";
        File.WriteAllText(sampleFile, "Hello World!\nThis is a file.\nAnother line.");

        ITextReader checker = new SmartTextChecker();
        var data = checker.ReadFile(sampleFile);
        checker.Close();

        ITextReader locker = new SmartTextReaderLocker(@"\.secret\.txt$");

        Console.WriteLine("\nTrying normal file (sample.txt):");
        locker.ReadFile("sample.txt");
        locker.Close();

        Console.WriteLine("\nTrying secret file (my.secret.txt):");
        locker.ReadFile("my.secret.txt");
        locker.Close();

    }
}