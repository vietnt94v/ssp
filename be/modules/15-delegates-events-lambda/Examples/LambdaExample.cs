namespace Learn.M15.Examples;

public static class LambdaExample
{
  public static void Run()
  {
    var nums = new[] { 1, 2, 3 };
    var doubled = nums.Select(n => n * 2);
    Console.WriteLine(string.Join(',', doubled));
  }
}
