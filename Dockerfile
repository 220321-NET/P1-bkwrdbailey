FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY . .

RUN dotnet clean ./StoreBack/P1-bkwrdbailey.sln
RUN dotnet publish ./StoreBack/WebAPI --configuration Release -o ./StoreBack/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS run

WORKDIR /app

COPY --from=build /app/StoreBack/publish .

CMD ["dotnet", "WebAPI.dll"]