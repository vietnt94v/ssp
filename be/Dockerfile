FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY global.json Directory.Build.props Ssp.Cmms.sln ./
COPY src/Ssp.Cmms.Domain/Ssp.Cmms.Domain.csproj src/Ssp.Cmms.Domain/
COPY src/Ssp.Cmms.Application/Ssp.Cmms.Application.csproj src/Ssp.Cmms.Application/
COPY src/Ssp.Cmms.Infrastructure/Ssp.Cmms.Infrastructure.csproj src/Ssp.Cmms.Infrastructure/
COPY src/Ssp.Cmms.Api/Ssp.Cmms.Api.csproj src/Ssp.Cmms.Api/
RUN dotnet restore src/Ssp.Cmms.Api/Ssp.Cmms.Api.csproj
COPY . .
RUN dotnet publish src/Ssp.Cmms.Api/Ssp.Cmms.Api.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh
ENTRYPOINT ["./entrypoint.sh"]
