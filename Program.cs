using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class ExtensionMethods
{
    public static T GetMax<T>(this IEnumerable collection, Func<T, float> convertToNumber) where T : class
    {
        T maxItem = null;
        float maxValue = float.MinValue;

        foreach (var item in collection)
        {
            T tItem = item as T;
            if (tItem == null) continue;

            float currentValue = convertToNumber(tItem);
            if (maxItem == null || currentValue > maxValue)
            {
                maxValue = currentValue;
                maxItem = tItem;
            }
        }
        return maxItem;
    }
}

public class FileArgs : EventArgs
{
    public string FileName { get; }
    public bool Cancel { get; set; }

    public FileArgs(string fileName)
    {
        FileName = fileName;
        Cancel = false;
    }
}

public class FileSearcher
{
    public event EventHandler<FileArgs> FileFound;

    public void Search(string directory)
    {
        try
        {
            foreach (var file in Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories))
            {
                var args = new FileArgs(file);
                OnFileFound(args);
                if (args.Cancel) return;
            }
        }
        catch (UnauthorizedAccessException) { }
        catch (DirectoryNotFoundException) { }
    }

    protected virtual void OnFileFound(FileArgs args)
    {
        FileFound?.Invoke(this, args);
    }
}

class Program
{
    static void Main()
    {
        ArrayList strings = new ArrayList { "apple", "banana", "cherry" };
        var maxString = strings.GetMax<string>(s => s.Length);
        Console.WriteLine($"Max string: {maxString}");

        var searcher = new FileSearcher();
        searcher.FileFound += (sender, args) =>
        {
            Console.WriteLine($"Found: {args.FileName}");
            if (args.FileName.EndsWith(".txt"))
            {
                Console.WriteLine("Stopping search...");
                args.Cancel = true;
            }
        };

        searcher.Search(@"C:\Example");
    }
}
