namespace Learn.M09.Examples;

public static class PropertiesExample
{
  public static void Run()
  {
    var t = new Thermometer();
    t.Celsius = 20;
    Console.WriteLine($"C={t.Celsius}, F={t.Fahrenheit}");
  }

  private sealed class Thermometer
  {
    private double celsius;

    public double Celsius
    {
      get => celsius;
      set => celsius = value;
    }

    public double Fahrenheit => celsius * 9 / 5 + 32;
  }
}
