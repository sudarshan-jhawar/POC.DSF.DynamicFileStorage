#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["POC.DSF.FileApp/POC.DSF.FileApp.csproj", "POC.DSF.FileApp/"]
RUN dotnet restore "POC.DSF.FileApp/POC.DSF.FileApp.csproj"
COPY . .
WORKDIR "/src/POC.DSF.FileApp"
RUN dotnet build "POC.DSF.FileApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "POC.DSF.FileApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "POC.DSF.FileApp.dll"]