namespace Learn.M07.Examples;

public static class InheritanceExample
{
  public static void Run()
  {
    var d = new Dog();
    Console.WriteLine(d.Species());
  }

  private class Animal
  {
    public virtual string Species()
    {
      return "animal";
    }
  }

  private sealed class Dog : Animal
  {
    public override string Species()
    {
      return "dog";
    }
  }
}
