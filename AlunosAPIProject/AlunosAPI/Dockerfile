# ==========================================
# Etapa 1 - Build
# ==========================================

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["AlunosAPI.csproj", "./"]

RUN dotnet restore "AlunosAPI.csproj"

COPY . .

RUN dotnet publish "AlunosAPI.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# ==========================================
# Etapa 2 - Runtime
# ==========================================

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "AlunosAPI.dll"]