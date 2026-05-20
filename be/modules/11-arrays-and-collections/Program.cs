using Learn.M11.Examples;

Console.WriteLine("Module 11 — Arrays and collections");
Console.WriteLine("1 ArrayExample");
Console.WriteLine("2 ListDictionaryExample");
Console.WriteLine("3 IEnumerableExample");
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
      ArrayExample.Run();
      break;
    case "2":
      ListDictionaryExample.Run();
      break;
    case "3":
      IEnumerableExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
