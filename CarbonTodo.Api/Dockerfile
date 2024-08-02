FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CarbonTodo.Api/CarbonTodo.Api.csproj", "CarbonTodo.Api/"]
COPY ["CarbonTodo.Infrastructure/CarbonTodo.Infrastructure.csproj", "CarbonTodo.Infrastructure/"]
COPY ["CarbonTodo.Domain/CarbonTodo.Domain.csproj", "CarbonTodo.Domain/"]
RUN dotnet restore "CarbonTodo.Api/CarbonTodo.Api.csproj"
COPY . .
WORKDIR "/src/CarbonTodo.Api"
RUN dotnet build "CarbonTodo.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CarbonTodo.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarbonTodo.Api.dll"]
