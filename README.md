# MIGRACIONES DB

1. Para agregar una migracion poner en terminal el comando:

   dotnet ef migrations add "00" --project nombreProyecto.csproj

2. Para actualizar la migracion actual poner en terminal el comando:

   dotnet ef database update --project Roulette.csproj
