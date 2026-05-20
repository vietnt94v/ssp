namespace Learn.M14.Examples;

public static class GenericClassExample
{
  public static void Run()
  {
    var box = new Box<int>(42);
    Console.WriteLine(box.Value);
  }

  private sealed class Box<T>
  {
    public Box(T value)
    {
      Value = value;
    }

    public T Value { get; }
  }
}
