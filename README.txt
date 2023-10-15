docker-compose build
docker-compose up

http://localhost:8000/api/values
http://localhost:8000/menu/auth/register/


https://localhost:44305/hangfire - дашборд hangfire
что бы выключить редис - просто не заполняем поля в конфиге - CACHE REDIS
уровень логов настраивается в nlog.config
включить\выключить эластик  в nlog.config, но настройки в appsettings





testmail@mail.com
testPass123!




обратить внимание на параметр PhysicalPath
при разработке он один, при деплое по идеи будет другой, можно проверить загрузив картинку и проверив что она там где надо


#db
при накатке первой миграции надо выключать исключения nloga потому что он пытается логировать в несозданную бд
throwExceptions="false"
 "Server=.\\SQLEXPRESS;Database=Menu-DataBase;Trusted_Connection=True;MultipleActiveResultSets=true"
после update database надо локально запустить приложение, это нужно что бы hangfire прорастил в бд свои таблицы. просто update не хватит

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
также есть ветка на гитхаб с небольшими изменениями

#redis
https://redis.io/docs/getting-started/install-stack/docker/
docker run -d --name redis-stack -p 6379:6379 -p 8001:8001 redis/redis-stack:latest


поставка
1 обновить конфиги:
nlog
appsettings
2 накат миграций
3 скопировать файлы с заменой(убедиться что картинки\uploads были сохранены)







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
















