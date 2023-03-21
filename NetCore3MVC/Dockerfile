#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["NetCore3MVC/NetCore3MVC.csproj", "NetCore3MVC/"]
RUN dotnet restore "NetCore3MVC/NetCore3MVC.csproj"
COPY . .
WORKDIR "/src/NetCore3MVC"
RUN dotnet build "NetCore3MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetCore3MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCore3MVC.dll"]