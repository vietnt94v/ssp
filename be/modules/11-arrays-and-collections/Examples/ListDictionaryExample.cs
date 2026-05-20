namespace Learn.M11.Examples;

public static class ListDictionaryExample
{
  public static void Run()
  {
    var list = new List<string> { "a", "b" };
    list.Add("c");
    var map = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 };
    map["z"] = 3;
    Console.WriteLine(string.Join(',', list));
    foreach (var kv in map)
    {
      Console.WriteLine($"{kv.Key}={kv.Value}");
    }
  }
}
