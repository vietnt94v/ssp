using Learn.M03.Examples;

Console.WriteLine("Module 03 — Operators and casting");
Console.WriteLine("1 ArithmeticExample");
Console.WriteLine("2 ComparisonLogicalExample");
Console.WriteLine("3 CastingExample");
Console.WriteLine("q Thoat");
while (true)
{
  Console.Write("> ");
  var line = Console.ReadLine();
  if (string.IsNullOrWhiteSpace(line))
  {
    continue;
  }

  if (line.Equals("q", StringComparison.OrdinalIgnoreCase))
  {
    break;
  }

  switch (line.Trim())
  {
    case "1":
      ArithmeticExample.Run();
      break;
    case "2":
      ComparisonLogicalExample.Run();
      break;
    case "3":
      CastingExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
