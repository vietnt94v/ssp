namespace Learn.M19.Examples;

public static class RecordExample
{
  public static void Run()
  {
    var a = new Point(1, 2);
    var b = a with { X = 9 };
    Console.WriteLine($"{a} | {b}");
  }

  private sealed record Point(int X, int Y);
}
