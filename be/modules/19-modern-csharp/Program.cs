using Learn.M19.Examples;

Console.WriteLine("Module 19 — Modern C#");
Console.WriteLine("1 RecordExample");
Console.WriteLine("2 PatternMatchingExample");
Console.WriteLine("3 NullableReferenceExample");
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
      RecordExample.Run();
      break;
    case "2":
      PatternMatchingExample.Run();
      break;
    case "3":
      NullableReferenceExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
