namespace Learn.M14.Examples;

public static class ConstraintsExample
{
  public static void Run()
  {
    var m = new Repo<Person>();
    Console.WriteLine(m.Describe(new Person("Nam")));
  }

  private interface IEntity
  {
    string Name { get; }
  }

  private sealed class Person : IEntity
  {
    public Person(string name)
    {
      Name = name;
    }

    public string Name { get; }
  }

  private sealed class Repo<T>
    where T : IEntity
  {
    public string Describe(T entity)
    {
      return entity.Name;
    }
  }
}
