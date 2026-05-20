namespace Learn.M18.Examples;

public static class PathExample
{
  public static void Run()
  {
    var p = Path.Combine("data", "logs", "app.log");
    Console.WriteLine(p);
  }
}
