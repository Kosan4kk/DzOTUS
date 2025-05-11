[TestFixture]
public class PrototypeTests
{
    [Test]
    public void EmployeeCloneTest()
    {
        var emp = new Employee("John", 30, "Engineer", 50000);
        var clone = emp.MyClone();

        Assert.AreNotSame(emp, clone);
        Assert.AreEqual(emp.Position, clone.Position);
    }

    [Test]
    public void ManagerCloneTest()
    {
        var manager = new Manager("Bob", 40, "CTO", 100000, "IT");
        var clone = (Manager)manager.Clone();

        Assert.AreEqual(manager.Department, clone.Department);
    }
}
