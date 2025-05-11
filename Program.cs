using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

public class F
{
    public int i1, i2, i3, i4, i5;
    public static F Get() => new F { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 };
}

public static class CsvSerializer
{
    private static readonly Dictionary<Type, FieldInfo[]> FieldCache = new();

    public static string Serialize(object obj)
    {
        var type = obj.GetType();
        if (!FieldCache.TryGetValue(type, out var fields))
        {
            fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                         .OrderBy(f => f.Name)
                         .ToArray();
            FieldCache[type] = fields;
        }
        return string.Join(",", fields.Select(f => f.GetValue(obj)));
    }

    public static T Deserialize<T>(string csv) where T : new()
    {
        var values = csv.Split(',');
        var obj = new T();
        var type = typeof(T);
        
        if (!FieldCache.TryGetValue(type, out var fields))
        {
            fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                         .OrderBy(f => f.Name)
                         .ToArray();
            FieldCache[type] = fields;
        }

        for (int i = 0; i < Math.Min(values.Length, fields.Length); i++)
        {
            if (int.TryParse(values[i], out int intValue))
                fields[i].SetValue(obj, intValue);
        }
        return obj;
    }
}

class Program
{
    static void Main()
    {
        F obj = F.Get();
        int iterations = 100000;

        // Замер сериализации CSV
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
            CsvSerializer.Serialize(obj);
        sw.Stop();
        long csvSerializeTime = sw.ElapsedMilliseconds;

        // Замер десериализации CSV
        string csv = "1,2,3,4,5";
        sw.Restart();
        for (int i = 0; i < iterations; i++)
            CsvSerializer.Deserialize<F>(csv);
        sw.Stop();
        long csvDeserializeTime = sw.ElapsedMilliseconds;

        // Замер JSON сериализации
        sw.Restart();
        for (int i = 0; i < iterations; i++)
            JsonConvert.SerializeObject(obj);
        sw.Stop();
        long jsonSerializeTime = sw.ElapsedMilliseconds;

        // Замер JSON десериализации
        string json = JsonConvert.SerializeObject(obj);
        sw.Restart();
        for (int i = 0; i < iterations; i++)
            JsonConvert.DeserializeObject<F>(json);
        sw.Stop();
        long jsonDeserializeTime = sw.ElapsedMilliseconds;

        // Вывод результатов
        Console.WriteLine($"Custom CSV Serialize: {csvSerializeTime} ms");
        Console.WriteLine($"Custom CSV Deserialize: {csvDeserializeTime} ms");
        Console.WriteLine($"Newtonsoft JSON Serialize: {jsonSerializeTime} ms");
        Console.WriteLine($"Newtonsoft JSON Deserialize: {jsonDeserializeTime} ms");

        // Замер времени вывода в консоль
        string output = CsvSerializer.Serialize(obj);
        sw.Restart();
        Console.WriteLine(output);
        sw.Stop();
        Console.WriteLine($"Console output time: {sw.ElapsedMilliseconds} ms");
    }
}
