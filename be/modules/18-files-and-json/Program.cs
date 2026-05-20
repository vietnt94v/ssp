using Learn.M18.Examples;

Console.WriteLine("Module 18 — Files and JSON");
Console.WriteLine("1 FileReadWriteExample");
Console.WriteLine("2 PathExample");
Console.WriteLine("3 SystemTextJsonExample");
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
      FileReadWriteExample.Run();
      break;
    case "2":
      PathExample.Run();
      break;
    case "3":
      SystemTextJsonExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
