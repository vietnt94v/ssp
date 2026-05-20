namespace Learn.M05.Examples;

public static class MethodBasicsExample
{
  public static void Run()
  {
    Console.WriteLine(Add(2, 3));
    Console.WriteLine(Greet("ban"));
  }

  private static int Add(int a, int b)
  {
    return a + b;
  }

  private static string Greet(string name)
  {
    return $"Xin chao, {name}!";
  }
}
