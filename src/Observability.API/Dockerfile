FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Observability.API/Observability.API.csproj", "Observability.API/"]
RUN dotnet restore "src/Observability.API/Observability.API.csproj"
COPY . .
WORKDIR "/src/Observability.API"
RUN dotnet build "Observability.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Observability.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Observability.API.dll"]
