# Stage build - SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore CarDealership.csproj
COPY . .
RUN dotnet publish CarDealership.csproj -c Release -o /out --no-restore /p:UseAppHost=false

# Stage runtime - doar ASP.NET, fara SDK
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
WORKDIR /app
COPY --from=build /out .
RUN (addgroup -S app 2>/dev/null || true) \
 && (adduser -S -G app app 2>/dev/null || true) \
 && mkdir -p /app/wwwroot/uploads \
 && chown -R app:app /app
USER app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "CarDealership.dll"]
