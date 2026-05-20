namespace Learn.M15.Examples;

public static class DelegateExample
{
  public static void Run()
  {
    Func<int, int, int> add = (a, b) => a + b;
    Action<string> log = s => Console.WriteLine(s);
    log(add(2, 3).ToString());
  }
}
