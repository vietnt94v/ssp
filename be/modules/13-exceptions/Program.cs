using Learn.M13.Examples;

Console.WriteLine("Module 13 — Exceptions");
Console.WriteLine("1 TryCatchExample");
Console.WriteLine("2 ThrowExample");
Console.WriteLine("3 UsingExceptionExample");
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
      TryCatchExample.Run();
      break;
    case "2":
      ThrowExample.Run();
      break;
    case "3":
      UsingExceptionExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
