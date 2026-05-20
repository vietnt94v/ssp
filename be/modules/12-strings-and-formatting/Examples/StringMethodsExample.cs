namespace Learn.M12.Examples;

public static class StringMethodsExample
{
  public static void Run()
  {
    var s = "  a,b,c  ";
    Console.WriteLine(s.Trim());
    Console.WriteLine(s.Contains("b"));
    Console.WriteLine(string.Join('|', s.Trim().Split(',')));
  }
}
