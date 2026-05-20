# Ssp.Learn — C# backend learning path

.NET 8 solution with numbered modules. Each folder under `modules/` is a runnable project.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Run a module (console)

```bash
cd be
dotnet run --project modules/01-fundamentals
```

## Run module 21 (ASP.NET Core)

```bash
cd be
dotnet run --project modules/21-aspnet-minimal-api
```

Then open `http://localhost:5000` (or the URL printed in the terminal).

## Module index

| Folder | Topic |
|--------|--------|
| [01-fundamentals](modules/01-fundamentals/README.md) | Hello world, namespaces, top-level statements |
| [02-types-and-variables](modules/02-types-and-variables/README.md) | Value/reference types, var, const, boxing |
| [03-operators-and-casting](modules/03-operators-and-casting/README.md) | Operators, casting, `is` / `as` |
| [04-control-flow](modules/04-control-flow/README.md) | if/else, switch, loops |
| [05-methods-and-parameters](modules/05-methods-and-parameters/README.md) | Methods, ref/out, overloads |
| [06-classes-and-objects](modules/06-classes-and-objects/README.md) | Classes, constructors |
| [07-oop-inheritance-polymorphism](modules/07-oop-inheritance-polymorphism/README.md) | Inheritance, virtual/override |
| [08-interfaces-and-abstract](modules/08-interfaces-and-abstract/README.md) | Interfaces, abstract classes |
| [09-encapsulation-and-properties](modules/09-encapsulation-and-properties/README.md) | Access modifiers, properties |
| [10-static-and-structs-enums](modules/10-static-and-structs-enums/README.md) | static, struct, enum |
| [11-arrays-and-collections](modules/11-arrays-and-collections/README.md) | Arrays, List, Dictionary |
| [12-strings-and-formatting](modules/12-strings-and-formatting/README.md) | String APIs, StringBuilder |
| [13-exceptions](modules/13-exceptions/README.md) | try/catch, throw |
| [14-generics](modules/14-generics/README.md) | Generic types and methods |
| [15-delegates-events-lambda](modules/15-delegates-events-lambda/README.md) | Func, Action, events |
| [16-linq](modules/16-linq/README.md) | LINQ query and method syntax |
| [17-async-await](modules/17-async-await/README.md) | async/await, Task |
| [18-files-and-json](modules/18-files-and-json/README.md) | File I/O, System.Text.Json |
| [19-modern-csharp](modules/19-modern-csharp/README.md) | Records, pattern matching, nullable reference types |
| [20-disposal-and-resources](modules/20-disposal-and-resources/README.md) | using, IDisposable |
| [21-aspnet-minimal-api](modules/21-aspnet-minimal-api/README.md) | Minimal API, DI, configuration |

## Suggested pace

Roughly one row per week for six weeks (see each module README). Adjust to your schedule.
