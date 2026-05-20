namespace Learn.M11.Examples;

public static class ArrayExample
{
  public static void Run()
  {
    var one = new[] { 1, 2, 3 };
    var grid = new int[2, 3];
    grid[0, 1] = 5;
    Console.WriteLine(string.Join(',', one));
    Console.WriteLine($"grid rows={grid.GetLength(0)} cols={grid.GetLength(1)}");
  }
}
