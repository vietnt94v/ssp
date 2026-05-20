using Learn.M04.Examples;

Console.WriteLine("Module 04 — Control flow");
Console.WriteLine("1 IfElseExample");
Console.WriteLine("2 SwitchExample");
Console.WriteLine("3 LoopsExample");
Console.WriteLine("4 BreakContinueExample");
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
      IfElseExample.Run();
      break;
    case "2":
      SwitchExample.Run();
      break;
    case "3":
      LoopsExample.Run();
      break;
    case "4":
      BreakContinueExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
