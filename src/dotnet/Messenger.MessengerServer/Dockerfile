#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["common/gRPC", "common/gRPC"]

WORKDIR /src/dotnet
COPY ["dotnet/Messenger.MessengerServer/Messenger.MessengerServer.csproj", "Messenger.MessengerServer/"]
COPY ["dotnet/MySql.Common/MySql.Common.csproj", "MySql.Common/"]
COPY ["dotnet/Messenger.Common/Messenger.Common.csproj", "Messenger.Common/"]
RUN dotnet restore "Messenger.MessengerServer/Messenger.MessengerServer.csproj"
COPY dotnet .
WORKDIR "/src/dotnet/Messenger.MessengerServer"
RUN dotnet build "Messenger.MessengerServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Messenger.MessengerServer.csproj" -c Release -o /app/publish

FROM base AS final
RUN apt-get update && apt-get install procps -y
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Messenger.MessengerServer.dll"]