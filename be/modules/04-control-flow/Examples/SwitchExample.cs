namespace Learn.M04.Examples;

public static class SwitchExample
{
  public static void Run()
  {
    var day = DayOfWeek.Monday;
    switch (day)
    {
      case DayOfWeek.Saturday:
      case DayOfWeek.Sunday:
        Console.WriteLine("Weekend (statement switch)");
        break;
      default:
        Console.WriteLine("Weekday");
        break;
    }

    var label = day switch
    {
      DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend (expression)",
      _ => "Weekday",
    };
    Console.WriteLine(label);
  }
}
