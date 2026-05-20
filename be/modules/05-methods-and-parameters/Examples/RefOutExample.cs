namespace Learn.M05.Examples;

public static class RefOutExample
{
  public static void Run()
  {
    var n = 1;
    Bump(ref n);
    Console.WriteLine($"sau ref: {n}");
    ParseDemo(out var ok, out var value);
    Console.WriteLine($"out ok={ok}, value={value}");
    PrintPair(1, 2);
  }

  private static void Bump(ref int x)
  {
    x++;
  }

  private static void ParseDemo(out bool ok, out int value)
  {
    ok = int.TryParse("42", out value);
  }

  private static void PrintPair(in int a, in int b)
  {
    Console.WriteLine($"in params: {a}, {b}");
  }
}
