http://localhost:8000/api/values
http://localhost:8000/menu/auth/register/


#db
 "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=Menu-DataBase;Trusted_Connection=True;MultipleActiveResultSets=true"
  },

docker pull mcr.microsoft.com/mssql/server:2017-CU11-ubuntu
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SqlServerSuperPassword2017" -p 1433:1433 --restart=always -d mcr.microsoft.com/mssql/server:2017-CU11-ubuntu
из студии коннект в окне "обозреватель объектов sql", сервер 127.0.0.1,1433


testmail@mail.com
testPass123!




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
















