FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Build.props ./
COPY global.json ./
COPY src/Ssp.Cmms.Domain/Ssp.Cmms.Domain.csproj src/Ssp.Cmms.Domain/
COPY src/Ssp.Cmms.Application/Ssp.Cmms.Application.csproj src/Ssp.Cmms.Application/
COPY src/Ssp.Cmms.Infrastructure/Ssp.Cmms.Infrastructure.csproj src/Ssp.Cmms.Infrastructure/
COPY src/Ssp.Cmms.Api/Ssp.Cmms.Api.csproj src/Ssp.Cmms.Api/
RUN dotnet restore src/Ssp.Cmms.Api/Ssp.Cmms.Api.csproj

COPY . .
RUN dotnet publish src/Ssp.Cmms.Api/Ssp.Cmms.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["sh", "-c", "export ASPNETCORE_URLS=http://+:${PORT:-8080} && exec dotnet Ssp.Cmms.Api.dll"]
