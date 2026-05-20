namespace Learn.M07.Examples;

public static class PolymorphismExample
{
  public static void Run()
  {
    Shape s1 = new Circle(2);
    Shape s2 = new Square(3);
    Console.WriteLine(s1.Area());
    Console.WriteLine(s2.Area());
  }

  private abstract class Shape
  {
    public abstract double Area();
  }

  private sealed class Circle : Shape
  {
    private readonly double r;

    public Circle(double r)
    {
      this.r = r;
    }

    public override double Area()
    {
      return Math.PI * r * r;
    }
  }

  private sealed class Square : Shape
  {
    private readonly double side;

    public Square(double side)
    {
      this.side = side;
    }

    public override double Area()
    {
      return side * side;
    }
  }
}
