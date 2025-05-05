FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY ./src/*.csproj ./
RUN dotnet restore

COPY ./src/ ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT [ "dotnet", "dotnet-simple-bank.dll" ]
