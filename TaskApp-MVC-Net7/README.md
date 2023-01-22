
# Aplicación de tareas con .NET 7


## Ejecutar migraciones para crear la base de datos

### Visual Studio 
1. Abrir la consola de paquetes NuGet 

2. Ejecutar el siguiente comando

	PM> Add-Migration Tasks

3. Ejecutar el siguiente comando
	PM> Update-Database

### Visual Studio Code
1. Abrir la consola en la carpeta del proyecto
 
2. Ejecutar el siguiente comando

	dotnet ef migrations add Tasks

3. Ejecutar el siguiente comando
	dotnet ef database update




### Notas

Remover ultimas migraciones 

(Visual Studio)
PM> Remove-Migration
or
(Visual Studio Code)
dotnet ef migrations remove