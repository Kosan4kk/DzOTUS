class Program
{
    static void Main()
    {
        // Пример клонирования через IMyCloneable
        var student = new Student("Tom", 20, "Computer Science", 4.5);
        var studentClone = student.MyClone();
        Console.WriteLine($"Student Clone: {studentClone.Name}, {studentClone.Age}");

        // Пример клонирования через ICloneable
        var manager = new Manager("Alice", 35, "CEO", 150000, "Management");
        var managerClone = (Manager)manager.Clone();
        Console.WriteLine($"Manager Clone: {managerClone.Department}");
    }
}
