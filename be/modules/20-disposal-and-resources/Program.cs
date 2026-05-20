using Learn.M20.Examples;

Console.WriteLine("Module 20 — Disposal and resources");
Console.WriteLine("1 UsingStatementExample");
Console.WriteLine("2 IDisposableExample");
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
      UsingStatementExample.Run();
      break;
    case "2":
      IDisposableExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
