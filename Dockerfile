# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY Payment.CKOBankClient src/Payment.CKOBankClient
COPY Payment.Utils src/Payment.Utils
COPY Payment.DB src/Payment.DB
COPY Payment.Logic src/Payment.Logic
COPY Payment.REST src/Payment.REST

RUN dotnet publish -c Release -o /app --use-current-runtime --self-contained false src/Payment.REST/Payment.REST.csproj

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Payment.REST.dll"]