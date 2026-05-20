namespace Learn.M18.Examples;

public static class FileReadWriteExample
{
  public static void Run()
  {
    var path = Path.Combine(Path.GetTempPath(), "learn-m18-demo.txt");
    File.WriteAllText(path, "hello");
    var text = File.ReadAllText(path);
    File.Delete(path);
    Console.WriteLine(text);
  }
}
