#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Application.Api/Application.Api.csproj", "src/Application.Api/"]
COPY ["src/Application.Services/Application.Services.csproj", "src/Application.Services/"]
COPY ["src/Common/Application.Common/Application.Common.csproj", "src/Common/Application.Common/"]
COPY ["src/Data/Application.Data.Master/Application.Data.Master.csproj", "src/Data/Application.Data.Master/"]
COPY ["src/Common/Application.Data.Common/Application.Data.Common.csproj", "src/Common/Application.Data.Common/"]
COPY ["src/Data/Application.Data.Tenant/Application.Data.Tenant.csproj", "src/Data/Application.Data.Tenant/"]
COPY ["src/Identity/Application.Identity.Services/Application.Identity.Services.csproj", "src/Identity/Application.Identity.Services/"]
COPY ["src/Identity/Application.Identity.Data/Application.Identity.Data.csproj", "src/Identity/Application.Identity.Data/"]
COPY ["src/Common/Application.Jobs/Application.Jobs.csproj", "src/Common/Application.Jobs/"]
COPY ["src/Contracts/Application.ExternalContracts/Application.ExternalContracts.csproj", "src/Contracts/Application.ExternalContracts/"]
COPY ["src/Contracts/Application.InternalContracts/Application.InternalContracts.csproj", "src/Contracts/Application.InternalContracts/"]
RUN dotnet restore "./src/Application.Api/./Application.Api.csproj"
COPY . .
WORKDIR "/src/src/Application.Api"
RUN dotnet build "./Application.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Application.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install the agent
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y 'newrelic-dotnet-agent' \
&& rm -rf /var/lib/apt/lists/*

# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=91ec172c1e23676502525be01383933dFFFFNRAL \
NEW_RELIC_APP_NAME="VistaLOS"


ENTRYPOINT ["dotnet", "VistaLOS.Application.Api.dll"]