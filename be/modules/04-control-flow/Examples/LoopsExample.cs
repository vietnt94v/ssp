namespace Learn.M04.Examples;

public static class LoopsExample
{
  public static void Run()
  {
    Console.Write("for: ");
    for (var i = 0; i < 3; i++)
    {
      Console.Write(i + " ");
    }

    Console.WriteLine();
    Console.Write("while: ");
    var j = 0;
    while (j < 3)
    {
      Console.Write(j + " ");
      j++;
    }

    Console.WriteLine();
    Console.Write("do-while: ");
    var k = 0;
    do
    {
      Console.Write(k + " ");
      k++;
    } while (k < 3);

    Console.WriteLine();
    Console.Write("foreach: ");
    foreach (var ch in "abc")
    {
      Console.Write(ch + " ");
    }

    Console.WriteLine();
  }
}
