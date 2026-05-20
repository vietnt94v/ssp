namespace Learn.M03.Examples;

public static class CastingExample
{
  public static void Run()
  {
    double d = 9.8;
    int i = (int)d;
    Console.WriteLine($"explicit (int){d} = {i}");
    long lng = i;
    Console.WriteLine($"implicit long: {lng}");
    object o = "text";
    var s = o as string;
    Console.WriteLine($"as string: {s}");
    Console.WriteLine($"is string: {o is string}");
  }
}
