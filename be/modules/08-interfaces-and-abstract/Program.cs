using Learn.M08.Examples;

Console.WriteLine("Module 08 — Interfaces and abstract");
Console.WriteLine("1 InterfaceExample");
Console.WriteLine("2 AbstractClassExample");
Console.WriteLine("3 DefaultInterfaceExample");
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
      InterfaceExample.Run();
      break;
    case "2":
      AbstractClassExample.Run();
      break;
    case "3":
      DefaultInterfaceExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
