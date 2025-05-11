public class Student : Person, IMyCloneable<Student>
{
    public string Major { get; set; }
    public double GPA { get; set; }

    public Student(string name, int age, string major, double gpa)
        : base(name, age)
    {
        Major = major;
        GPA = gpa;
    }

    public override Person MyClone() => new Student(Name, Age, Major, GPA);

    Student IMyCloneable<Student>.MyClone() => (Student)MyClone();

    public new object Clone() => ((IMyCloneable<Student>)this).MyClone();
}
