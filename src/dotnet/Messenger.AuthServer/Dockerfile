#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Messenger.AuthServer/Messenger.AuthServer.csproj", "Messenger.AuthServer/"]
COPY ["Messenger.Common/Messenger.Common.csproj", "Messenger.Common/"]
RUN dotnet restore "Messenger.AuthServer/Messenger.AuthServer.csproj"
COPY . .
WORKDIR "/src/Messenger.AuthServer"
RUN dotnet build "Messenger.AuthServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Messenger.AuthServer.csproj" -c Release -o /app/publish

FROM base AS final
RUN apt-get update && apt-get install procps -y
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Messenger.AuthServer.dll"]