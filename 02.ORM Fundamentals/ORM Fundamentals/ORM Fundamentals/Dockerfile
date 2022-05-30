#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ORM Fundamentals/ORM Fundamentals.csproj", "ORM Fundamentals/"]
RUN dotnet restore "ORM Fundamentals/ORM Fundamentals.csproj"
COPY . .
WORKDIR "/src/ORM Fundamentals"
RUN dotnet build "ORM Fundamentals.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ORM Fundamentals.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ORM Fundamentals.dll"]