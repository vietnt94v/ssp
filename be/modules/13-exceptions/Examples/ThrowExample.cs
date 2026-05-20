namespace Learn.M13.Examples;

public static class ThrowExample
{
  public static void Run()
  {
    try
    {
      EnsurePositive(-1);
    }
    catch (ArgumentOutOfRangeException ex)
    {
      Console.WriteLine(ex.Message);
    }
  }

  private static void EnsurePositive(int value)
  {
    if (value < 0)
    {
      throw new ArgumentOutOfRangeException(nameof(value), "Phai >= 0");
    }
  }
}
