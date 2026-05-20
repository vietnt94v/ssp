namespace Learn.M09.Examples;

public static class AccessModifiersExample
{
  public static void Run()
  {
    var bag = new Bag();
    bag.SetSecret(99);
    Console.WriteLine(bag.PublicPeek());
    InternalHelper.Print();
  }

  private sealed class Bag
  {
    private int secret;

    public void SetSecret(int value)
    {
      secret = value;
    }

    public int PublicPeek()
    {
      return secret;
    }
  }

  internal static class InternalHelper
  {
    public static void Print()
    {
      Console.WriteLine("internal type in cung assembly");
    }
  }
}
