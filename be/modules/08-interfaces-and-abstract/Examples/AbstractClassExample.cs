namespace Learn.M08.Examples;

public static class AbstractClassExample
{
  public static void Run()
  {
    Widget w = new LabelWidget("OK");
    Console.WriteLine(w.Render());
  }

  private abstract class Widget
  {
    public abstract string Render();
  }

  private sealed class LabelWidget : Widget
  {
    private readonly string text;

    public LabelWidget(string text)
    {
      this.text = text;
    }

    public override string Render()
    {
      return $"<label>{text}</label>";
    }
  }
}
