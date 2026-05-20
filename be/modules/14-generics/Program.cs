using Learn.M14.Examples;

Console.WriteLine("Module 14 — Generics");
Console.WriteLine("1 GenericClassExample");
Console.WriteLine("2 GenericMethodExample");
Console.WriteLine("3 ConstraintsExample");
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
      GenericClassExample.Run();
      break;
    case "2":
      GenericMethodExample.Run();
      break;
    case "3":
      ConstraintsExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
