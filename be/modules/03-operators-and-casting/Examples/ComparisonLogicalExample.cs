namespace Learn.M03.Examples;

public static class ComparisonLogicalExample
{
  public static void Run()
  {
    int x = 3;
    int y = 7;
    Console.WriteLine($"x==y: {x == y}, x<y: {x < y}");
    Console.WriteLine($"x>0 && y>0: {x > 0 && y > 0}");
    Console.WriteLine($"x==0 || y==7: {x == 0 || y == 7}");
    Console.WriteLine($"!(x==y): {!(x == y)}");
  }
}
