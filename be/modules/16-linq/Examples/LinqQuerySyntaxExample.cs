namespace Learn.M16.Examples;

public static class LinqQuerySyntaxExample
{
  public static void Run()
  {
    var nums = new[] { 3, 1, 4, 1, 5 };
    var q =
      from n in nums
      where n > 2
      orderby n
      select n * 10;
    Console.WriteLine(string.Join(',', q));
  }
}
