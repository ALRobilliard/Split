FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY . .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet split.dll