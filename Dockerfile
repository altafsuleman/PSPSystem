FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /build

# copy csproj and restore as distinct layers for caching
COPY src/Api/Api.csproj ./src/Api/Api.csproj
COPY src/Domain/Domain.csproj ./src/Domain/Domain.csproj
COPY src/Application/Application.csproj ./src/Application/Application.csproj
COPY src/Infrastructure/Infrastructure.csproj ./src/Infrastructure/Infrastructure.csproj
COPY src/PaymentAcquirer/PaymentAcquirer.csproj ./src/PaymentAcquirer/PaymentAcquirer.csproj
COPY Directory.Build.props .
RUN dotnet restore -r linux-x64 "./src/Api/Api.csproj"

# copy and publish app and libraries
COPY src/Api/ ./src/Api/
COPY src/Domain/ ./src/Domain/
COPY src/Application/ ./src/Application/
COPY src/Infrastructure/ ./src/Infrastructure/
COPY src/PaymentAcquirer/ ./src/PaymentAcquirer/
RUN dotnet publish -c Release --no-self-contained -r linux-x64 -o /app "./src/Api/Api.csproj"

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_URLS=http://+:5001
EXPOSE 5001
WORKDIR /app
COPY --from=build /app .
USER app
ENTRYPOINT ["dotnet", "WebApiTemplate.Api.dll"]
