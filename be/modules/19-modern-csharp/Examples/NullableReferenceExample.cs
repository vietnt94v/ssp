namespace Learn.M19.Examples;

public static class NullableReferenceExample
{
  public static void Run()
  {
    string? maybe = null;
    Console.WriteLine(maybe ?? "mac dinh");
    string definite = maybe ?? "fallback";
    Console.WriteLine(definite.Length);
    string? label = "x";
    Console.WriteLine(label!.Length);
  }
}
