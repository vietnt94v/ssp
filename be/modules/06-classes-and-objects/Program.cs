using Learn.M06.Examples;

Console.WriteLine("Module 06 — Classes and objects");
Console.WriteLine("1 ClassBasicsExample");
Console.WriteLine("2 ConstructorExample");
Console.WriteLine("3 ObjectInitializerExample");
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
      ClassBasicsExample.Run();
      break;
    case "2":
      ConstructorExample.Run();
      break;
    case "3":
      ObjectInitializerExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
