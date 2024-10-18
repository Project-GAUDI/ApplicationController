FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY ./ ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0-bookworm-slim
WORKDIR /app
COPY --from=build-env /app/out ./

USER root
COPY templates/entrypoint.sh ./
RUN chmod +x ./entrypoint.sh

ARG TAGS
RUN { \
    echo "module name : ApplicationController"; \
    echo "module version : ${TAGS}"; \
} > ./application.info

RUN useradd -ms /bin/bash moduleuser
USER moduleuser

# LABEL org.opencontainers.image.source=https://github.com/Project-GAUDI/ApplicationController
LABEL org.opencontainers.image.description="ApplicationController is an Azure IoT edge module that triggers message reception, executes a pre-configured process and outputs results."
# LABEL org.opencontainers.image.licenses=MIT

ENTRYPOINT ["/app/entrypoint.sh"]
