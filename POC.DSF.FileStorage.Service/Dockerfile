#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["POC.DSF.FileStorage.Service/POC.DSF.FileStorage.Service.csproj", "POC.DSF.FileStorage.Service/"]
RUN dotnet restore "POC.DSF.FileStorage.Service/POC.DSF.FileStorage.Service.csproj"
COPY . .
WORKDIR "/src/POC.DSF.FileStorage.Service"
RUN dotnet build "POC.DSF.FileStorage.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "POC.DSF.FileStorage.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "POC.DSF.FileStorage.Service.dll"]