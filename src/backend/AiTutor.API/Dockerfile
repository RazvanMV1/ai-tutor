FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/backend/AiTutor.API/AiTutor.API.csproj", "src/backend/AiTutor.API/"]
COPY ["src/backend/AiTutor.Application/AiTutor.Application.csproj", "src/backend/AiTutor.Application/"]
COPY ["src/backend/AiTutor.Domain/AiTutor.Domain.csproj", "src/backend/AiTutor.Domain/"]
COPY ["src/backend/AiTutor.Infrastructure/AiTutor.Infrastructure.csproj", "src/backend/AiTutor.Infrastructure/"]

RUN dotnet restore "src/backend/AiTutor.API/AiTutor.API.csproj"

COPY . .

RUN dotnet build "src/backend/AiTutor.API/AiTutor.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/backend/AiTutor.API/AiTutor.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AiTutor.API.dll"]
