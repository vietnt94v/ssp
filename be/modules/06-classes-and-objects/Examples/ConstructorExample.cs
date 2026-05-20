namespace Learn.M06.Examples;

public static class ConstructorExample
{
  public static void Run()
  {
    var a = new User("An");
    var b = new User("Binh", "binh@example.com");
    Console.WriteLine($"{a.Name} {a.Email}");
    Console.WriteLine($"{b.Name} {b.Email}");
  }

  private sealed class User
  {
    public User(string name)
      : this(name, "unknown@local")
    {
    }

    public User(string name, string email)
    {
      Name = name;
      Email = email;
    }

    public string Name { get; }
    public string Email { get; }
  }
}
