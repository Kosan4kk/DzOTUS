public class Employee : Person, IMyCloneable<Employee>
{
    public string Position { get; set; }
    public decimal Salary { get; set; }

    public Employee(string name, int age, string position, decimal salary)
        : base(name, age)
    {
        Position = position;
        Salary = salary;
    }

    // Переопределение метода MyClone (ковариантность)
    public override Person MyClone() => new Employee(Name, Age, Position, Salary);

    // Явная реализация для IMyCloneable<Employee>
    Employee IMyCloneable<Employee>.MyClone() => (Employee)MyClone();

    // Переопределение стандартного Clone
    public new object Clone() => ((IMyCloneable<Employee>)this).MyClone();
}
