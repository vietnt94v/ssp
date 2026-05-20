using Learn.M07.Examples;

Console.WriteLine("Module 07 — Inheritance and polymorphism");
Console.WriteLine("1 InheritanceExample");
Console.WriteLine("2 OverrideExample");
Console.WriteLine("3 PolymorphismExample");
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
      InheritanceExample.Run();
      break;
    case "2":
      OverrideExample.Run();
      break;
    case "3":
      PolymorphismExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
