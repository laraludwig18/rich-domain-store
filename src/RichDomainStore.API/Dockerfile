FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /src
COPY ["src/RichDomainStore.API/RichDomainStore.API.csproj", "src/RichDomainStore.API/"]
COPY ["src/EventSourcing/EventSourcing.csproj", "src/EventSourcing/"]
COPY ["src/RichDomainStore.Catalog.Application/RichDomainStore.Catalog.Application.csproj", "src/RichDomainStore.Catalog.Application/"]
COPY ["src/RichDomainStore.Catalog.Data/RichDomainStore.Catalog.Data.csproj", "src/RichDomainStore.Catalog.Data/"]
COPY ["src/RichDomainStore.Catalog.Domain/RichDomainStore.Catalog.Domain.csproj", "src/RichDomainStore.Catalog.Domain/"]
COPY ["src/RichDomainStore.Core/RichDomainStore.Core.csproj", "src/RichDomainStore.Core/"]
COPY ["src/RichDomainStore.Payments.AntiCorruption/RichDomainStore.Payments.AntiCorruption.csproj", "src/RichDomainStore.Payments.AntiCorruption/"]
COPY ["src/RichDomainStore.Payments.Business/RichDomainStore.Payments.Business.csproj", "src/RichDomainStore.Payments.Business/"]
COPY ["src/RichDomainStore.Payments.Data/RichDomainStore.Payments.Data.csproj", "src/RichDomainStore.Payments.Data/"]
COPY ["src/RichDomainStore.Sales.Application/RichDomainStore.Sales.Application.csproj", "src/RichDomainStore.Sales.Application/"]
COPY ["src/RichDomainStore.Sales.Data/RichDomainStore.Sales.Data.csproj", "src/RichDomainStore.Sales.Data/"]
COPY ["src/RichDomainStore.Sales.Domain/RichDomainStore.Sales.Domain.csproj", "src/RichDomainStore.Sales.Domain/"]

RUN dotnet restore "src/RichDomainStore.API/RichDomainStore.API.csproj"

COPY . .
WORKDIR "/src/src/RichDomainStore.API"
RUN dotnet build "RichDomainStore.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RichDomainStore.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "RichDomainStore.API.dll"]
