FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["AiTutor.Web.csproj", "./"]
RUN dotnet restore "AiTutor.Web.csproj"

COPY . .
RUN dotnet publish "AiTutor.Web.csproj" -c Release -o /app/publish

FROM node:20-alpine AS final
WORKDIR /app

RUN npm install -g http-server

COPY --from=build /app/publish/wwwroot .

EXPOSE 3000
CMD ["http-server", ".", "-p", "3000", "--cors", "-c-1"]
