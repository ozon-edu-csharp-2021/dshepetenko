FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /src

COPY ["MerchandiseService/MerchandiseService.csproj","src/MerchandiseService/"] 

RUN dotnet restore "MerchandiseService/MerchandiseService.csproj"

COPY .. .

WORKDIR "src/src/MerchandiseService"

RUN dotnet build "MerchandiseService.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "MerchandiseService.csproj" -c Release -o /app/publish

COPY "../entrypoint.sh" "/app/publish/."

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runtime

WORKDIR /app
EXPOSE 80

FROM runtime as final
WORKDIR /app

COPY --from=publish /app/publish .

RUN chmod +x entrypoint.sh
ENTRYPOINT /bin/bash entrypoint.sh
