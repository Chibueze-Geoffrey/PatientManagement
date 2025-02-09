# Base Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /PatientManagement
COPY ["PatientManagement.Api/PatientManagement.Api.csproj", "PatientManagement.Api/"]
COPY ["PatientManagement.Domain/PatientManagement.Domain.csproj", "PatientManagement.Domain/"]
COPY ["PatientManagement.Common/PatientManagement.Common.csproj", "PatientManagement.Common/"]
COPY ["PatientManagement.Application/PatientManagement.Application.csproj", "PatientManagement.Application/"]
COPY ["PatientManagement.Infrastructure/PatientManagement.Infrastructure.csproj", "PatientManagement.Infrastructure/"]
RUN dotnet restore "PatientManagement.Api/PatientManagement.Api.csproj"
COPY . .
WORKDIR "/PatientManagement/PatientManagement.Api"
RUN dotnet build "PatientManagement.Api.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "PatientManagement.Api.csproj" -c Release -o /app/publish

# Final Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Use dynamic port
CMD ["sh", "-c", "dotnet PatientManagement.Api.dll --urls=http://0.0.0.0:${PORT}"]
