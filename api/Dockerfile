FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5080

ENV ASPNETCORE_URLS=http://+:5080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY TaskTracker.sln ./

COPY TaskTracker.Web/*.csproj ./TaskTracker.Web/
COPY TaskTracker.Common/*.csproj ./TaskTracker.Common/
COPY TaskTracker.Data/*.csproj ./TaskTracker.Data/
COPY TaskTracker.Tests/*.csproj ./TaskTracker.Tests/

RUN dotnet restore
COPY . .
WORKDIR /src/TaskTracker.Common
RUN dotnet build -c Release -o /app
WORKDIR /src/TaskTracker.Data
RUN dotnet build -c Release -o /app
WORKDIR /src/TaskTracker.Tests
RUN dotnet build -c Release -o /app

WORKDIR /src/TaskTracker.Web
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TaskTracker.Web.dll"]
