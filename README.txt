http://localhost:8000/api/values
https://zsuz-menu.azurewebsites.net/menu/auth/register/

 "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=Menu-DataBase;Trusted_Connection=True;MultipleActiveResultSets=true"
  },


testmail@mail.com
testPass123!


https://github.com/dotnet/dotnet-docker/tree/master/samples/aspnetapp
docker volume create --name menu-volume --opt type=none --opt device=c:/menu-volume --opt o=bind
docker build --rm --pull -t menu .
docker run --rm -it -p 8000:80 menu --mount source=menu-volume,target=/app/wwwroot


docker run --rm -it -p 8000:80 menu -v //c/Users/Stanislav_Avdosev/source/repos/MenuBack/menu-volume:/test


http://localhost:8000/menu


map path azure js blob
/app/wwwroot/js/tmp
















