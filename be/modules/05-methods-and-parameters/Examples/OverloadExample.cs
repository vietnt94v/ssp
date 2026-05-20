namespace Learn.M05.Examples;

public static class OverloadExample
{
  public static void Run()
  {
    Console.WriteLine(Format(7));
    Console.WriteLine(Format(3.14));
    Console.WriteLine(Format("text"));
  }

  private static string Format(int x)
  {
    return $"int:{x}";
  }

  private static string Format(double x)
  {
    return $"double:{x}";
  }

  private static string Format(string x)
  {
    return $"string:{x}";
  }
}
