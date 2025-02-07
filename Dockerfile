# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /PatientManagement
COPY ["PatientManagement.Api/PatientManagement.Api.csproj", "PatientManagement.Api/"]
COPY ["PatientManagement.Domain/PatientManagement.Domain.csproj", "PatientManagement.Domain/"]
COPY ["PatientManagement.Application/PatientManagement.Application.csproj", "PatientManagement.Application/"]
COPY ["PatientManagement.Infrastructure/PatientManagement.Infrastructure.csproj", "PatientManagement.Infrastructure/"]
RUN dotnet restore "PatientManagement.Api/PatientManagement.Api.csproj"
COPY . .
WORKDIR "/PatientManagement/PatientManagement.Api"
RUN dotnet build "PatientManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PatientManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatientManagement.Api.dll"]
