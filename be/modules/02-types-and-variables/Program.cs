using Learn.M02.Examples;

Console.WriteLine("Module 02 — Types and variables");
Console.WriteLine("1 ValueTypesExample");
Console.WriteLine("2 ReferenceTypesExample");
Console.WriteLine("3 VarAndConstExample");
Console.WriteLine("4 BoxingExample");
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
      ValueTypesExample.Run();
      break;
    case "2":
      ReferenceTypesExample.Run();
      break;
    case "3":
      VarAndConstExample.Run();
      break;
    case "4":
      BoxingExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
