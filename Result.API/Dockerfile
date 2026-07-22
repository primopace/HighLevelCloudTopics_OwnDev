# ---------- BUILD STAGE ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy entire solution
COPY . .

# Restore
RUN dotnet restore "Result.API/Result.API.csproj"

# Publish
RUN dotnet publish "Result.API/Result.API.csproj" -c Release -o /app/publish

# ---------- RUNTIME STAGE ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Result.API.dll"]