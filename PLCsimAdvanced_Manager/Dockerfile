FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PLCsimAdvanced_Manager/PLCsimAdvanced_Manager.csproj", "PLCsimAdvanced_Manager/"]
RUN dotnet restore "PLCsimAdvanced_Manager/PLCsimAdvanced_Manager.csproj"
COPY . .
WORKDIR "/src/PLCsimAdvanced_Manager"
RUN dotnet build "PLCsimAdvanced_Manager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PLCsimAdvanced_Manager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PLCsimAdvanced_Manager.dll"]
