using Learn.M09.Examples;

Console.WriteLine("Module 09 — Encapsulation and properties");
Console.WriteLine("1 AccessModifiersExample");
Console.WriteLine("2 PropertiesExample");
Console.WriteLine("3 InitExample");
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
      AccessModifiersExample.Run();
      break;
    case "2":
      PropertiesExample.Run();
      break;
    case "3":
      InitExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
