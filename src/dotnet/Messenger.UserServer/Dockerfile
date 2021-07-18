#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Messenger.UserServer/Messenger.UserServer.csproj", "Messenger.UserServer/"]
COPY ["MySql.Common/MySql.Common.csproj", "MySql.Common/"]
COPY ["Messenger.Common/Messenger.Common.csproj", "Messenger.Common/"]
RUN dotnet restore "Messenger.UserServer/Messenger.UserServer.csproj"
COPY . .
WORKDIR "/src/Messenger.UserServer"
RUN dotnet build "Messenger.UserServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Messenger.UserServer.csproj" -c Release -o /app/publish

FROM base AS final
RUN apt-get update && apt-get install procps -y
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Messenger.UserServer.dll"]