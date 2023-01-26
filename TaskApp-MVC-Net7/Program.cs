using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json.Serialization;
using TaskApp;
using TaskApp.Servicios;
using TaskApp.Servicios.Constantes.Auth;

var builder = WebApplication.CreateBuilder(args);



// Pol�tica de autentificaci�n | Filtro global
var politicaAutenticados = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();


// Add services to the container.
// se a�ade la pol�tica de autentificaci�n a todos los controladores
builder.Services.AddControllersWithViews(opciones =>
{
    opciones.Filters.Add(new AuthorizeFilter(politicaAutenticados));
}).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
   .AddDataAnnotationsLocalization(opciones =>
   {
       opciones.DataAnnotationLocalizerProvider = (_, factoria) => factoria.Create(typeof(RecursoCompartido)); // utilizar un �nico archivo de recursos
   })
   .AddJsonOptions(opt =>
   {
       opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Ignorar las referencias c�clicas, por ejemplo Tareas Tiene Pasos , Pasos tienen una tarea
   }); 

// DB Service
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("name=DefaultConnection"));

// Habilitar los servicios de autentificaci�n
builder.Services.AddAuthentication()
    .AddMicrosoftAccount(opciones =>
    {
        opciones.ClientId = builder.Configuration["MicrosoftClientId"];
        opciones.ClientSecret = builder.Configuration["MicrosoftSecretId"];
    }
    );

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opciones =>
{
    opciones.SignIn.RequireConfirmedAccount = false; // No requiero una cuenta confirmada para iniciar sesi�n
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


// Trabajar con mis propias vistas de autentificaci�n
builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opciones =>
{
    opciones.LoginPath = "/usuarios/login";
    opciones.AccessDeniedPath = "/usuarios/login";
});

// Internacionalizaci�n de la aplicaci�n
builder.Services.AddLocalization(opciones =>
{
    opciones.ResourcesPath = "Resources";
});

builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>();

// configuraci�n de automapper
builder.Services.AddAutoMapper(typeof(Program));
// Configuraci�n cargar archivos
builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();

var app = builder.Build();

//var culturasUISoportadas = new[] { "es", "en" };


app.UseRequestLocalization(opciones =>
{
    opciones.DefaultRequestCulture = new RequestCulture("es");
    opciones.SupportedUICultures = Constantes.CulturasUISoportadas.Select(cultura => new CultureInfo(cultura.Value)).ToList();
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
