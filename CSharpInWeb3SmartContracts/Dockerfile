#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CSharpInWeb3SmartContracts/CSharpInWeb3SmartContracts.csproj", "CSharpInWeb3SmartContracts/"]
RUN dotnet restore "CSharpInWeb3SmartContracts/CSharpInWeb3SmartContracts.csproj"
COPY . .
WORKDIR "/src/CSharpInWeb3SmartContracts"
RUN dotnet build "CSharpInWeb3SmartContracts.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CSharpInWeb3SmartContracts.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CSharpInWeb3SmartContracts.dll"]