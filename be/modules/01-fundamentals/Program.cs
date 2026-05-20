using Learn.M01.Examples;

Console.WriteLine("Module 01 — Fundamentals");
Console.WriteLine("1 HelloWorldExample");
Console.WriteLine("2 NamespaceExample");
Console.WriteLine("3 TopLevelStatementsExample");
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
      HelloWorldExample.Run();
      break;
    case "2":
      NamespaceExample.Run();
      break;
    case "3":
      TopLevelStatementsExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
