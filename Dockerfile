FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ARG PORT=80
ENV PORT "$PORT"
ENV ASPNETCORE_URLS=http://+:$PORT
RUN echo "$ASPNETCORE_URLS"
WORKDIR /app
EXPOSE $PORT

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WebApplication4/WebApplication4.csproj", "WebApplication4/"]
RUN dotnet restore "WebApplication4/WebApplication4.csproj"
COPY . .
WORKDIR "/src/WebApplication4"
RUN dotnet build "WebApplication4.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApplication4.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplication4.dll"]
