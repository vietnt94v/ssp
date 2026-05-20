using Learn.M15.Examples;

Console.WriteLine("Module 15 — Delegates, events, lambda");
Console.WriteLine("1 DelegateExample");
Console.WriteLine("2 LambdaExample");
Console.WriteLine("3 EventExample");
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
      DelegateExample.Run();
      break;
    case "2":
      LambdaExample.Run();
      break;
    case "3":
      EventExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
