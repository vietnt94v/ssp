using Learn.M10.Examples;

Console.WriteLine("Module 10 — Static, struct, enum");
Console.WriteLine("1 StaticExample");
Console.WriteLine("2 StructExample");
Console.WriteLine("3 EnumExample");
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
      StaticExample.Run();
      break;
    case "2":
      StructExample.Run();
      break;
    case "3":
      EnumExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
