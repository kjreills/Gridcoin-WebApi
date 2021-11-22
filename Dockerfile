#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["Gridcoin.WebApi/Gridcoin.WebApi.csproj", "."]
RUN dotnet restore "./Gridcoin.WebApi.csproj"
COPY "Gridcoin.WebApi" .
WORKDIR "/src/."
RUN dotnet build "Gridcoin.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gridcoin.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gridcoin.WebApi.dll"]
