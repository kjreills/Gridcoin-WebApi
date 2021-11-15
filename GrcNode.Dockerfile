
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base

# ADD CODE HERE TO DOWNLOAD/BUILD GRIDCOIN NODE HERE

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["Gridcoin.WebApi.csproj", "."]
RUN dotnet restore "./Gridcoin.WebApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Gridcoin.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gridcoin.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gridcoin.WebApi.dll"]