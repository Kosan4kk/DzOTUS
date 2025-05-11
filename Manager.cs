public class Manager : Employee, IMyCloneable<Manager>
{
    public string Department { get; set; }

    public Manager(string name, int age, string position, decimal salary, string department)
        : base(name, age, position, salary)
    {
        Department = department;
    }

    public override Person MyClone() => new Manager(Name, Age, Position, Salary, Department);

    Manager IMyCloneable<Manager>.MyClone() => (Manager)MyClone();

    public new object Clone() => ((IMyCloneable<Manager>)this).MyClone();
}
