namespace Learn.M02.Examples;

public static class VarAndConstExample
{
  private const int Max = 100;
  private static readonly DateTime Started = DateTime.UtcNow;

  public static void Run()
  {
    var inferred = "var suy luan kieu";
    Console.WriteLine($"{inferred}, const Max={Max}, readonly Started={Started:O}");
  }
}
