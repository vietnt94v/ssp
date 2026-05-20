namespace Learn.M05.Examples;

public static class ParametersExample
{
  public static void Run()
  {
    Log("mac dinh");
    Log("tuy chinh", prefix: "[APP]");
    Log(prefix: "[DBG]", message: "ping");
  }

  private static void Log(string message, string prefix = "[LOG]")
  {
    Console.WriteLine($"{prefix} {message}");
  }
}
