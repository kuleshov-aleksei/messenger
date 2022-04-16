#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt update && apt install -y procps
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src/dotnet
COPY ["Messenger.Common/Messenger.Common.csproj", "Messenger.Common/"]
COPY ["Messenger.InstantMessagingService/Messenger.InstantMessagingService.csproj", "Messenger.InstantMessagingService/"]
RUN dotnet restore "Messenger.InstantMessagingService/Messenger.InstantMessagingService.csproj"
COPY . .
WORKDIR "/src/dotnet/Messenger.InstantMessagingService"
RUN dotnet build "Messenger.InstantMessagingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Messenger.InstantMessagingService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Messenger.InstantMessagingService.dll"]