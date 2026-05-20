namespace Learn.M17.Examples;

public static class CancellationExample
{
  public static async Task RunAsync()
  {
    using var cts = new CancellationTokenSource();
    cts.CancelAfter(100);
    try
    {
      await Task.Delay(2000, cts.Token);
    }
    catch (OperationCanceledException)
    {
      Console.WriteLine("da huy");
    }
  }
}
