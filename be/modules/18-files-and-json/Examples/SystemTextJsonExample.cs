using System.Text.Json;

namespace Learn.M18.Examples;

public static class SystemTextJsonExample
{
  public static void Run()
  {
    var obj = new PersonDto { Name = "Nam", Age = 20 };
    var json = JsonSerializer.Serialize(obj);
    var back = JsonSerializer.Deserialize<PersonDto>(json);
    Console.WriteLine(json);
    Console.WriteLine(back?.Name);
  }

  private sealed class PersonDto
  {
    public string Name { get; set; } = "";
    public int Age { get; set; }
  }
}
