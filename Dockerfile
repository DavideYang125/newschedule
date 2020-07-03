#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM registry.cn-shanghai.aliyuncs.com/yangww/dotnetruntime3.1 AS base
FROM registry.cn-shanghai.aliyuncs.com/yangww/dotnetruntime:v1.0 AS base
WORKDIR /app
EXPOSE 80

#FROM registry.cn-qingdao.aliyuncs.com/uguobapublic/dotnetsdk3.1 AS build
FROM registry.cn-shanghai.aliyuncs.com/yangww/dotnetsdk:v1.0 AS build
WORKDIR /src
COPY ["src/NewSchedule/NewSchedule.csproj", "src/NewSchedule/"]
RUN dotnet restore "src/NewSchedule/NewSchedule.csproj"
COPY . .
WORKDIR "/src/src/NewSchedule"
RUN dotnet build "NewSchedule.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NewSchedule.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NewSchedule.dll"]