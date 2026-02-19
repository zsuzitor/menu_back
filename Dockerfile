#some info
#about sln in docker https://stackoverflow.com/questions/56150932/docker-file-skipping-project-because-it-was-not-found
# https://stackoverflow.com/questions/47103570/asp-net-core-2-0-multiple-projects-solution-docker-file
#https://github.com/dotnet/dotnet-docker/tree/master/samples/dotnetapp
#https://docs.docker.com/engine/examples/dotnetcore/

#https://github.com/dotnet/dotnet-docker/issues/1649
#https://github.com/dotnet/dotnet-docker/issues/1309



# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
#FROM mcr.microsoft.com/dotnet/sdk:3.1 AS base
WORKDIR /app

# copy csproj and restore as distinct layers, копируем только проекты для того что бы пакеты могли закешироваться, и не грузиться при любом изменении cs
COPY *src/.sln ./
COPY ["src/Auth/*.csproj", "Auth/"]
COPY ["src/BL/*.csproj", "BL/"]
COPY ["src/BO/*.csproj", "BO/"]
COPY ["src/Common/*.csproj", "Common/"]
COPY ["src/DAL/*.csproj", "DAL/"]
COPY ["src/WEB.Common/*.csproj", "WEB.Common/"]

COPY ["src/Menu/Menu/*.csproj", "Menu/"]
COPY ["src/MenuApp/MenuApp/*.csproj", "MenuApp/"]
COPY ["src/WordsCardsApp/WordsCardsApp/*.csproj", "WordsCardsApp/"]
COPY ["src/PlaningPokerApp/PlanitPoker/*.csproj", "PlanitPoker/"]
COPY ["src/TaskManagementApp/TaskManagementApp/*.csproj", "TaskManagementApp/"]
COPY ["src/VaultApp/VaultApp/*.csproj", "VaultApp/"]


RUN dotnet restore

COPY ["src/Auth/", "Auth/"]
COPY ["src/BL/", "BL/"]
COPY ["src/BO/", "BO/"]
COPY ["src/Common/", "Common/"]
COPY ["src/DAL/", "DAL/"]
COPY ["src/WEB.Common/", "WEB.Common/"]


COPY ["src/Menu/Menu/", "Menu/"]
COPY ["src/MenuApp/MenuApp/", "MenuApp/"]
COPY ["src/WordsCardsApp/WordsCardsApp/", "WordsCardsApp/"]
COPY ["src/PlanitPoker/", "PlanitPoker/"]
COPY ["src/TaskManagementApp/TaskManagementApp/", "TaskManagementApp/"]
COPY ["src/VaultApp/VaultApp/", "VaultApp/"]


#что бы не запускать тут билд отдельной командой, надо добавить игнор на все bin obj папки и другой мусор
#RUN dotnet build  -o /app/out
WORKDIR /app/Menu
RUN dotnet publish -c release -o /app/out --no-restore

# final stage/image
#FROM mcr.microsoft.com/dotnet/runtime:3.1
FROM mcr.microsoft.com/dotnet/core/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out ./
#VOLUME menu-volume
ENTRYPOINT ["dotnet", "Menu.dll"]