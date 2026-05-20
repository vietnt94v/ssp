namespace Learn.M08.Examples;

public static class DefaultInterfaceExample
{
  public static void Run()
  {
    IGreeter g = new FormalGreeter();
    g.SayHello();
    g.SayHello("Alex");
  }

  private interface IGreeter
  {
    void SayHello()
    {
      Console.WriteLine("Hello!");
    }

    void SayHello(string name);
  }

  private sealed class FormalGreeter : IGreeter
  {
    public void SayHello(string name)
    {
      Console.WriteLine($"Good day, {name}.");
    }
  }
}
