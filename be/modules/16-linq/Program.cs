using Learn.M16.Examples;

Console.WriteLine("Module 16 — LINQ");
Console.WriteLine("1 LinqQuerySyntaxExample");
Console.WriteLine("2 LinqMethodSyntaxExample");
Console.WriteLine("3 LinqAggregationExample");
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
      LinqQuerySyntaxExample.Run();
      break;
    case "2":
      LinqMethodSyntaxExample.Run();
      break;
    case "3":
      LinqAggregationExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
