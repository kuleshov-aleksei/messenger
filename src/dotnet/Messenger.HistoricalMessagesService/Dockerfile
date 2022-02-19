#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src/dotnet
COPY ["MySql.Common/MySql.Common.csproj", "MySql.Common/"]
COPY ["Messenger.Common/Messenger.Common.csproj", "Messenger.Common/"]
COPY ["Messenger.HistoricalMessagesService/Messenger.HistoricalMessagesService.csproj", "Messenger.HistoricalMessagesService/"]
RUN dotnet restore "Messenger.HistoricalMessagesService/Messenger.HistoricalMessagesService.csproj"
COPY . .
WORKDIR "/src/dotnet/Messenger.HistoricalMessagesService"
RUN dotnet build "Messenger.HistoricalMessagesService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Messenger.HistoricalMessagesService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Messenger.HistoricalMessagesService.dll"]