namespace Learn.M09.Examples;

public static class InitExample
{
  public static void Run()
  {
    var p1 = new Profile { Name = "Nam", Role = "dev" };
    Console.WriteLine($"{p1.Name} {p1.Role}");
  }

  private sealed class Profile
  {
    public required string Name { get; init; }
    public string Role { get; init; } = "guest";
  }
}
