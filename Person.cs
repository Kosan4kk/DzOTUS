public class Person : IMyCloneable<Person>, ICloneable
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    // Реализация пользовательского интерфейса
    public virtual Person MyClone() => new Person(Name, Age);

    // Реализация стандартного интерфейса
    public object Clone() => MyClone();
}
