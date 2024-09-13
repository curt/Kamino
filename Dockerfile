# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG TARGETARCH
WORKDIR /build

# copy and publish app and libraries
COPY ./src ./src
RUN dotnet publish ./src/Kamino.Endpoint/Kamino.Endpoint.csproj -a $TARGETARCH -o /app

# Enable globalization and time zones:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/enable-globalization.md
# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
LABEL org.opencontainers.image.source https://github.com/curt/Kamino
RUN apk --no-cache add postgresql-client
EXPOSE 8080
WORKDIR /app
COPY --from=build /app .
COPY ./docker-entrypoint.sh .
ENTRYPOINT ./docker-entrypoint.sh
