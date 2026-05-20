namespace Learn.M04.Examples;

public static class BreakContinueExample
{
  public static void Run()
  {
    Console.Write("continue (bo qua 2): ");
    for (var i = 0; i < 5; i++)
    {
      if (i == 2)
      {
        continue;
      }

      Console.Write(i + " ");
    }

    Console.WriteLine();
    Console.Write("break (dung o 3): ");
    for (var i = 0; i < 10; i++)
    {
      if (i == 3)
      {
        break;
      }

      Console.Write(i + " ");
    }

    Console.WriteLine();
  }
}
