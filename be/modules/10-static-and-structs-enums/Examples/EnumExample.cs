namespace Learn.M10.Examples;

public static class EnumExample
{
  [Flags]
  private enum Permissions
  {
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4,
  }

  public static void Run()
  {
    var p = Permissions.Read | Permissions.Write;
    Console.WriteLine(p);
    Console.WriteLine(p.HasFlag(Permissions.Write));
  }
}
