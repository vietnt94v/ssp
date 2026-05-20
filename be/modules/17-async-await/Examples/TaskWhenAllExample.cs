namespace Learn.M17.Examples;

public static class TaskWhenAllExample
{
  public static async Task RunAsync()
  {
    var a = Task.Delay(100);
    var b = Task.Delay(150);
    await Task.WhenAll(a, b);
    Console.WriteLine("both finished");
  }
}
