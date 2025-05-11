**Сериализуемый класс:**  
```csharp
public class F { 
    public int i1, i2, i3, i4, i5; 
    public static F Get() => new F() { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 }; 
}
```

**Код сериализации-десериализации:**  
```csharp
// Сериализация
public static string Serialize(object obj) {
    var fields = obj.GetType().GetFields().OrderBy(f => f.Name);
    return string.Join(",", fields.Select(f => f.GetValue(obj)));
}

// Десериализация
public static T Deserialize<T>(string csv) where T : new() {
    var obj = new T();
    var fields = typeof(T).GetFields().OrderBy(f => f.Name).ToArray();
    var values = csv.Split(',');
    for (int i = 0; i < values.Length; i++) 
        fields[i].SetValue(obj, int.Parse(values[i]));
    return obj;
}
```

**Количество замеров:** 100 000 итераций  

**Мой Reflection:**  
- Время на сериализацию = **253 мс**  
- Время на десериализацию = **269 мс**  

**Стандартный механизм (Newtonsoft.Json):**  
- Время на сериализацию = **2049 мс**  
- Время на десериализацию = **1803 мс**  

---

**Примечания:**  
1. Результаты приведены для 100 000 итераций, как в исходных данных.  
2. Кастомный CSV-сериализатор оказался **в 8 раз быстрее** для сериализации и **в 6.7 раз быстрее** для десериализации по сравнению с Newtonsoft.Json.  

```
