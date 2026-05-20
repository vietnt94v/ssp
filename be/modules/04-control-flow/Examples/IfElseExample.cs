namespace Learn.M04.Examples;

public static class IfElseExample
{
  public static void Run()
  {
    int score = 72;
    if (score >= 90)
    {
      Console.WriteLine("A");
    }
    else if (score >= 70)
    {
      Console.WriteLine("B");
    }
    else
    {
      Console.WriteLine("C");
    }
  }
}
