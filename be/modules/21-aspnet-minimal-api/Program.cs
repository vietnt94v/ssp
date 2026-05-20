using Learn.M21.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<GreetingService>();

var app = builder.Build();

app.MapGet("/", (GreetingService greeter) => Results.Text(greeter.Hello()));
app.MapGet("/config", (IConfiguration config) =>
{
  var course = config["Learn:CourseName"] ?? "(missing)";
  return Results.Text($"Learn:CourseName = {course}");
});

app.Run();
