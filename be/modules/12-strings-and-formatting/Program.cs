using Learn.M12.Examples;

Console.WriteLine("Module 12 — Strings and formatting");
Console.WriteLine("1 StringMethodsExample");
Console.WriteLine("2 StringBuilderExample");
Console.WriteLine("3 InterpolationExample");
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
      StringMethodsExample.Run();
      break;
    case "2":
      StringBuilderExample.Run();
      break;
    case "3":
      InterpolationExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
