http://localhost:8000/api/values
http://localhost:8000/menu/auth/register/







testmail@mail.com
testPass123!


logLevel
указывается в 3х местах
Program
nlog config
appsettings config


#db
при накатке первй миграции надо выключать исключения nloga потому что он пытается логировать в несозданную бд
throwExceptions="false"
 "Server=.\\SQLEXPRESS;Database=Menu-DataBase;Trusted_Connection=True;MultipleActiveResultSets=true"

строка подключения находится в appsettings + nlog.config

#mssql
Server=127.0.0.1,1433;Database=Menu-DataBase;User Id=SA;Password=SqlServerSuperPassword2017
docker pull mcr.microsoft.com/mssql/server:2017-CU11-ubuntu
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SqlServerSuperPassword2017" -p 1433:1433 --restart=always -d mcr.microsoft.com/mssql/server:2017-CU11-ubuntu
из студии коннект в окне "обозреватель объектов sql", сервер 127.0.0.1,1433


#postgresql
docker run --name menu-pg-13.3 -p 5432:5432 -e POSTGRES_USER=pgusermenu -e POSTGRES_PASSWORD=pgpwd4menu -e POSTGRES_DB=menu -d postgres:13.3
"Host=localhost;Port=5432;Database=menu;Username=pgusermenu;Password=pgpwd4menu"
при миграции с mssql на postgres надо грохнуть все миграции и снапшоты и перестроить заного









----------------
что то старое не буду удалять


https://github.com/dotnet/dotnet-docker/tree/master/samples/aspnetapp
docker volume create --name menu-volume --opt type=none --opt device=c:/menu-volume --opt o=bind
docker build --rm --pull -t menu .
docker run --rm -it -p 8000:80 menu --mount source=menu-volume,target=/app/wwwroot


docker run --rm -it -p 8000:80 menu -v //c/Users/Stanislav_Avdosev/source/repos/MenuBack/menu-volume:/test


http://localhost:8000/menu


map path azure js blob
/app/wwwroot/js/tmp
















