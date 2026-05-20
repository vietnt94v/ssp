using Learn.M05.Examples;

Console.WriteLine("Module 05 — Methods and parameters");
Console.WriteLine("1 MethodBasicsExample");
Console.WriteLine("2 ParametersExample");
Console.WriteLine("3 RefOutExample");
Console.WriteLine("4 OverloadExample");
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
      MethodBasicsExample.Run();
      break;
    case "2":
      ParametersExample.Run();
      break;
    case "3":
      RefOutExample.Run();
      break;
    case "4":
      OverloadExample.Run();
      break;
    default:
      Console.WriteLine("Khong co vi du nay.");
      break;
  }
}
