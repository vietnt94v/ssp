namespace Learn.M13.Examples;

public static class UsingExceptionExample
{
  public static void Run()
  {
    try
    {
      Demo(shouldThrow: true);
    }
    catch (Exception ex) when (ex.Message.Contains("demo"))
    {
      Console.WriteLine($"Caught with filter: {ex.Message}");
    }
  }

  private static void Demo(bool shouldThrow)
  {
    if (shouldThrow)
    {
      throw new InvalidOperationException("demo loi");
    }
  }
}
