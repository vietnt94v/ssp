using System.Text;

namespace Learn.M12.Examples;

public static class StringBuilderExample
{
  public static void Run()
  {
    var sb = new StringBuilder();
    for (var i = 0; i < 5; i++)
    {
      sb.Append(i);
    }

    Console.WriteLine(sb.ToString());
  }
}
