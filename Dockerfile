#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/application/IotGatewayServer/IotGatewayServer.csproj", "src/application/IotGatewayServer/"]
RUN dotnet restore "src/application/IotGatewayServer/IotGatewayServer.csproj"
COPY . .

WORKDIR "/src/src/application/IotGatewayServer"
RUN dotnet build "IotGatewayServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IotGatewayServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IotGatewayServer.dll"]