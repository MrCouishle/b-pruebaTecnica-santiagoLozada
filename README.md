# MIGRACIONES DB

1. Para agregar una migracion tiene que entrar al directorio Models y poner en terminal el comando:

   dotnet ef migrations add "00" --project ../nombreProyecto

2. Para actualizar la migracion actual tiene que entrar al directorio Models y poner en terminal el comando:
   dotnet ef database update --project ../nombreProyecto
