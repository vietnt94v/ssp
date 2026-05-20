namespace Learn.M17.Examples;

public static class TaskDelayExample
{
  public static async Task RunAsync()
  {
    Console.WriteLine("start");
    await Task.Delay(200);
    Console.WriteLine("done");
  }
}
