namespace Learn.M08.Examples;

public static class InterfaceExample
{
  public static void Run()
  {
    IReadable a = new Book("C#");
    IReadable b = new Article("LINQ");
    Console.WriteLine($"{a.Title} / {b.Title}");
  }

  private interface IReadable
  {
    string Title { get; }
  }

  private sealed class Book : IReadable
  {
    public Book(string title)
    {
      Title = title;
    }

    public string Title { get; }
  }

  private sealed class Article : IReadable
  {
    public Article(string title)
    {
      Title = title;
    }

    public string Title { get; }
  }
}
