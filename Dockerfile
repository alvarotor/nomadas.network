FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Nomadas.Network/Nomadas.Network.csproj", "Nomadas.Network/"]
RUN dotnet restore "Nomadas.Network/Nomadas.Network.csproj"
COPY . .
WORKDIR /src/Nomadas.Network
RUN dotnet build "Nomadas.Network.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nomadas.Network.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nomadas.Network.dll"]
