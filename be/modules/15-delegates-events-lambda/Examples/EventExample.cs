namespace Learn.M15.Examples;

public static class EventExample
{
  public static void Run()
  {
    var pub = new Publisher();
    pub.MessagePrinted += msg => Console.WriteLine($"sub: {msg}");
    pub.Publish("hello");
  }

  private sealed class Publisher
  {
    public event Action<string>? MessagePrinted;

    public void Publish(string msg)
    {
      MessagePrinted?.Invoke(msg);
    }
  }
}
