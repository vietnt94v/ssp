namespace Learn.M16.Examples;

public static class LinqMethodSyntaxExample
{
  public static void Run()
  {
    var names = new[] { "An", "Binh", "Chi" };
    var ordered = names.Where(n => n.Length <= 3).OrderBy(n => n).Select(n => n.ToUpperInvariant());
    Console.WriteLine(string.Join(',', ordered));
  }
}
