namespace Learn.M02.Examples;

public static class ValueTypesExample
{
  public static void Run()
  {
    int count = 7;
    double ratio = 0.25;
    bool ok = true;
    char letter = 'A';
    decimal money = 19.99m;
    Console.WriteLine($"int {count}, double {ratio}, bool {ok}, char {letter}, decimal {money}");
  }
}
