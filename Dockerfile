#some info
#about sln in docker https://stackoverflow.com/questions/56150932/docker-file-skipping-project-because-it-was-not-found
# https://stackoverflow.com/questions/47103570/asp-net-core-2-0-multiple-projects-solution-docker-file
#https://github.com/dotnet/dotnet-docker/tree/master/samples/dotnetapp
#https://docs.docker.com/engine/examples/dotnetcore/

#https://github.com/dotnet/dotnet-docker/issues/1649
#https://github.com/dotnet/dotnet-docker/issues/1309



# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
#FROM mcr.microsoft.com/dotnet/sdk:3.1 AS base
WORKDIR /app

# copy csproj and restore as distinct layers, копируем только проекты для того что бы пакеты могли закешироваться, и не грузиться при любом изменении cs
COPY *.sln ./
COPY ["Auth/*.csproj", "Auth/"]
COPY ["BL/*.csproj", "BL/"]
COPY ["BO/*.csproj", "BO/"]
COPY ["Common/*.csproj", "Common/"]
COPY ["DAL/*.csproj", "DAL/"]
COPY ["WEB.Common/*.csproj", "WEB.Common/"]

COPY ["Menu/*.csproj", "Menu/"]
COPY ["MenuApp/*.csproj", "MenuApp/"]
COPY ["WordsCardsApp/*.csproj", "WordsCardsApp/"]
COPY ["Menu.Tests/*.csproj", "Menu.Tests/"]
COPY ["PlanitPoker/*.csproj", "PlanitPoker/"]
COPY ["CodeReviewApp/*.csproj", "CodeReviewApp/"]
COPY ["VaultApp/*.csproj", "VaultApp/"]


RUN dotnet restore

COPY ["Auth/", "Auth/"]
COPY ["BL/", "BL/"]
COPY ["BO/", "BO/"]
COPY ["Common/", "Common/"]
COPY ["DAL/", "DAL/"]
COPY ["WEB.Common/", "WEB.Common/"]


COPY ["Menu/", "Menu/"]
COPY ["MenuApp/", "MenuApp/"]
COPY ["WordsCardsApp/", "WordsCardsApp/"]
COPY ["Menu.Tests/", "Menu.Tests/"]
COPY ["PlanitPoker/", "PlanitPoker/"]
COPY ["CodeReviewApp/", "CodeReviewApp/"]
COPY ["VaultApp/", "VaultApp/"]


#что бы не запускать тут билд отдельной командой, надо добавить игнор на все bin obj папки и другой мусор
#RUN dotnet build  -o /app/out
WORKDIR /app/Menu
RUN dotnet publish -c release -o /app/out --no-restore

# final stage/image
#FROM mcr.microsoft.com/dotnet/runtime:3.1
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/out ./
#VOLUME menu-volume
ENTRYPOINT ["dotnet", "Menu.dll"]