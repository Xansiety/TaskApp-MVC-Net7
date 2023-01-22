
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



# Login Externo

Es importante que contemos con una cuenta de Azure, si no la tienes puedes crear una cuenta gratuita en el siguiente enlace [https://azure.microsoft.com/es-es/free/](https://azure.microsoft.com/es-es/free/)

## Configuración de Azure

1. Ingresar a [https://portal.azure.com/](https://portal.azure.com/)
2. Crear un nuevo registro de aplicaciones
	
	1. Proporcionar un nombre
	2. Seleccionar la opción de **Accounts in any organizational directory (Any Azure AD directory - Multitenant) and personal Microsoft accounts (e.g. Skype, Xbox)**
	3. Crear un nuevo recurso de tipo Aplicación Web (Web app / API) y proporcionar la siguiente URL: **http://localhost:{yourPort}/signin-microsoft**
	3. Seleccionar la opción de **Register**
	
3. Esperar a que finalice la creación

4. Copiar el **Application (client) ID** y el **Directory (tenant) ID** en algún lugar

5. Crear una nueva clave secreta

	1. Seleccionar la opción de **New client secret**
	2. Proporcionar una descripción
	3. Seleccionar la opción de **Add**
	4. Copiar el valor de la clave secreta en algún lugar (nameApp Secret)

6. Crear Secret Clients

	Teniendo la configuración anterior, debemos de apoyarnos de un paquete de nugget para poder leer los secretos de usuario,
	a su vez de que nos apoyara con la configuración para poder realizar el login externo

	1. Instalar el paquete de nugget: Microsoft.AspNetCore.Authentification.MicrosoftAccount
	2. Crearemos los secretos de usuario
		1. Dar click sobre el proyecto
		2. Seleccionar la opción de **Manage User Secrets**
		3. En el Json que se abrió, agregar la siguiente configuración
		``` json
			{
				"MicrosoftClientId": "YOUR_CLIENT_ID",
				"MicrosoftSecretId": "YOUR_MICROSOFT_SECRET_ID"
			}
		```
	3. Configurar la clase program de la siguiente forma
		``` csharp
		builder.Services.AddAuthentication()
		.AddMicrosoftAccount(opciones =>
			{
				opciones.ClientId = builder.Configuration["MicrosoftClientId"];
				opciones.ClientSecret = builder.Configuration["MicrosoftSecretId"];
			}
		);
	```
	
	4. Finalmente añadir el botón de login en la vista de login
		``` cshtml
		<a asp-action="LoginExterno" asp-route-proveedor="Microsoft" class="btn btn-primary btn-block">
                <i class="fab fa-microsoft"></i> Login con Microsoft
        </a>
		```
	

# Internacionalización - IStringLocalizer

Nos permite tener una aplicación multilenguaje / multiples idiomas, para ello debemos de seguir los siguientes pasos:

1. Configurar la localización en el archivo Startup.cs
	``` csharp
		builder.Services.AddLocalization();
	```
2. Configurar en el controlador el IStringLocalizer, inyectarlo desde el constructor

	``` csharp
		private readonly IStringLocalizer<HomeController> _localizer;
		public HomeController(IStringLocalizer<HomeController> localizer)
		{
			_localizer = localizer;
		}
	```

	3. En el controlador podemos usar a modo de ejemplo lo siguiente:
	``` csharp
		public IActionResult Index()
        {
            ViewBag.Saludo = localizer["Buenos días"];
            return View();
        }
	```

	4. En la vista podemos usar a modo de ejemplo lo siguiente:
	``` cshtml
		@{
			ViewData["Title"] = "Home Page";
		}

		<div class="text-center">
			<h1 class="display-4">@ViewBag.Saludo</h1>
		</div>
	```
	

 
	


### Notas

Remover ultimas migraciones 

(Visual Studio)
PM> Remove-Migration
or
(Visual Studio Code)
dotnet ef migrations remove