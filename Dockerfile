# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
RUN sed -i 's/dl-cdn.alpinelinux.org/mirrors.tencent.com/g' /etc/apk/repositories
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY aspnetapp/*.csproj ./aspnetapp/
COPY HighRiskDiseaseSurvellance.Aplication/*.csproj ./HighRiskDiseaseSurvellance.Aplication/
COPY HighRiskDiseaseSurvellance.Domain/*.csproj ./HighRiskDiseaseSurvellance.Domain/
COPY HighRiskDiseaseSurvellance.Dto/*.csproj ./HighRiskDiseaseSurvellance.Dto/
COPY HighRiskDiseaseSurvellance.Infrastructure/*.csproj ./HighRiskDiseaseSurvellance.Infrastructure/
COPY HighRiskDiseaseSurvellance.Persistence/*.csproj ./HighRiskDiseaseSurvellance.Persistence/
COPY OAuth.Adapter.WeChat/*.csproj ./OAuth.Adapter.WeChat/
RUN dotnet restore -r linux-musl-x64 /p:PublishReadyToRun=true

# copy everything else and build app
COPY aspnetapp/. ./aspnetapp/
COPY HighRiskDiseaseSurvellance.Aplication/. ./HighRiskDiseaseSurvellance.Aplication/
COPY HighRiskDiseaseSurvellance.Domain/. ./HighRiskDiseaseSurvellance.Domain/
COPY HighRiskDiseaseSurvellance.Dto/. ./HighRiskDiseaseSurvellance.Dto/
COPY HighRiskDiseaseSurvellance.Infrastructure/. ./HighRiskDiseaseSurvellance.Infrastructure/
COPY HighRiskDiseaseSurvellance.Persistence/. ./HighRiskDiseaseSurvellance.Persistence/
COPY OAuth.Adapter.WeChat/. ./OAuth.Adapter.WeChat/
WORKDIR /source/aspnetapp
RUN dotnet publish -c release -o /app -r linux-musl-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine-amd64

# 容器默认时区为UTC，如需使用上海时间请启用以下时区设置命令
# RUN apk add tzdata && cp /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo Asia/Shanghai > /etc/timezone

# 使用 HTTPS 协议访问容器云调用证书安装
RUN apk add ca-certificates

RUN sed -i 's/dl-cdn.alpinelinux.org/mirrors.tencent.com/g' /etc/apk/repositories
WORKDIR /app
COPY --from=build /app ./

# See: https://github.com/dotnet/announcements/issues/20
# Uncomment to enable globalization APIs (or delete)
# ENV \
#     DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
#     LC_ALL=en_US.UTF-8 \
#     LANG=en_US.UTF-8
# RUN apk add --no-cache icu-libs

ENTRYPOINT ["./aspnetapp"]
