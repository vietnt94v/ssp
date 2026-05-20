namespace Learn.M02.Examples;

public static class BoxingExample
{
  public static void Run()
  {
    int value = 10;
    object boxed = value;
    int unboxed = (int)boxed;
    Console.WriteLine($"value={value}, boxed={boxed}, unboxed={unboxed}");
  }
}
