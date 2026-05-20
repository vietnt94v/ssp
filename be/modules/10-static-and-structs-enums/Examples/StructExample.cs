namespace Learn.M10.Examples;

public static class StructExample
{
  public static void Run()
  {
    var a = new Point2D(1, 2);
    var b = a;
    b.X = 9;
    Console.WriteLine($"a={a.X},{a.Y} b={b.X},{b.Y}");
  }

  private struct Point2D
  {
    public int X;
    public int Y;

    public Point2D(int x, int y)
    {
      X = x;
      Y = y;
    }
  }
}
