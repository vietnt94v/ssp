namespace Learn.M02.Examples;

public static class ReferenceTypesExample
{
  public static void Run()
  {
    string name = "C#";
    object boxed = 42;
    string? maybe = null;
    Console.WriteLine($"string: {name}, object: {boxed}, nullable string: {maybe ?? "(null)"}");
  }
}
