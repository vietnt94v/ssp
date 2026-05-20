namespace Learn.M06.Examples;

public static class ClassBasicsExample
{
  public static void Run()
  {
    var p = new Point(2, 3);
    p.Move(1, 1);
    Console.WriteLine(p.Describe());
  }

  private sealed class Point
  {
    private int x;
    private int y;

    public Point(int x, int y)
    {
      this.x = x;
      this.y = y;
    }

    public void Move(int dx, int dy)
    {
      x += dx;
      y += dy;
    }

    public string Describe()
    {
      return $"({x},{y})";
    }
  }
}
