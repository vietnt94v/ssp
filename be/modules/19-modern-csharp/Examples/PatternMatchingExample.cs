namespace Learn.M19.Examples;

public static class PatternMatchingExample
{
  public static void Run()
  {
    object value = "hi";
    if (value is string s)
    {
      Console.WriteLine($"is pattern: {s}");
    }

    Console.WriteLine(Describe(7));
    Console.WriteLine(Describe(2.5));
  }

  private static string Describe(object o)
  {
    return o switch
    {
      int n when n > 5 => $"so lon {n}",
      int n => $"so {n}",
      double d => $"double {d}",
      _ => "khac",
    };
  }
}
