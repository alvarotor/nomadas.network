FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
LABEL Alvaro T (goodbytes23@gmail.com)
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
WORKDIR /src
RUN dotnet test
WORKDIR /src
RUN dotnet build "Nomadas.Network/Nomadas.Network.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nomadas.Network/Nomadas.Network.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nomadas.Network.dll"]
