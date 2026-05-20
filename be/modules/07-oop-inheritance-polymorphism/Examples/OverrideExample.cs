namespace Learn.M07.Examples;

public static class OverrideExample
{
  public static void Run()
  {
    Base b = new Derived();
    Console.WriteLine(b.Message());
  }

  private class Base
  {
    public virtual string Message()
    {
      return "base";
    }
  }

  private sealed class Derived : Base
  {
    public override string Message()
    {
      return "derived";
    }
  }
}
