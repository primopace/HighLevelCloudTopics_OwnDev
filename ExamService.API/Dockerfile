# ---------- BUILD STAGE ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

# Restore
RUN dotnet restore "ExamService.API/Exam.API.csproj"

# Publish
RUN dotnet publish "ExamService.API/Exam.API.csproj" -c Release -o /app/publish

# ---------- RUNTIME STAGE ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Exam.API.dll"]