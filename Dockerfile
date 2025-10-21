FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ./ .

RUN dotnet restore "c#.csproj"

RUN dotnet publish "c#.csproj" -c Release -o /app/publish /p:SuppressTrimAnalysis=true

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS="http://+:5049"
EXPOSE 5049

ENTRYPOINT ["dotnet", "c#.dll"]
