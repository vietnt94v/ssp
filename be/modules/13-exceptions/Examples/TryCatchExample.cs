namespace Learn.M13.Examples;

public static class TryCatchExample
{
  public static void Run()
  {
    try
    {
      var x = int.Parse("abc");
      Console.WriteLine(x);
    }
    catch (FormatException ex)
    {
      Console.WriteLine($"FormatException: {ex.Message}");
    }
    finally
    {
      Console.WriteLine("finally chay luon");
    }
  }
}
