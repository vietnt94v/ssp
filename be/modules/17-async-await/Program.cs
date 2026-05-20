using Learn.M17.Examples;

Console.WriteLine("Module 17 — Async and await");
Console.WriteLine("1 TaskDelayExample");
Console.WriteLine("2 TaskWhenAllExample");
Console.WriteLine("3 CancellationExample");
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
      await TaskDelayExample.RunAsync();
      break;
    case "2":
      await TaskWhenAllExample.RunAsync();
      break;
    case "3":
      await CancellationExample.RunAsync();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
