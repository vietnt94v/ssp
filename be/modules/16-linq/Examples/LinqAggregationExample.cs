namespace Learn.M16.Examples;

public static class LinqAggregationExample
{
  public static void Run()
  {
    var nums = new[] { 2, 3, 5 };
    Console.WriteLine($"count={nums.Count()} sum={nums.Sum()} first={nums.First()}");
  }
}
