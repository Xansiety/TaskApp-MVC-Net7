using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskApp_MVC_Net7.Models;
using TaskApp_MVC_Net7.Models.Auth;
using TaskApp_MVC_Net7.Servicios.Constantes.Auth;

namespace TaskApp_MVC_Net7.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext context;

        public UsuariosController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [AllowAnonymous]
        public IActionResult Registro() => View();


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            var usuario = new IdentityUser() { Email = modelo.Email, UserName = modelo.Email };
            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);
            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(modelo);
        }


        [AllowAnonymous]
        public IActionResult Login(string mensaje = null)
        {
            if (mensaje is not null)
            {
                ViewData["mensaje"] = mensaje;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid) return View(modelo);

            var resultado = await signInManager.PasswordSignInAsync(modelo.Email, modelo.Password, modelo.Recordarme, lockoutOnFailure: false);

            if (resultado.Succeeded) return RedirectToAction("Index", "Home");

            ModelState.AddModelError(string.Empty, "Datos incorrectos");

            return View(modelo);
        }


        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Login", "Usuarios");
        }



        // Login externo
        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult LoginExterno(string proveedor, string urlRetorno = null)
        {
            var urlRedireccion = Url.Action("RegistroUsuarioExterno", values: new { urlRetorno });
            var propiedades = signInManager.ConfigureExternalAuthenticationProperties(proveedor, urlRedireccion);
            return new ChallengeResult(proveedor, propiedades);
        }


        [AllowAnonymous]
        public async Task<IActionResult> RegistroUsuarioExterno(string urlRetorno = null, string remoteError = null)
        {
            urlRetorno = urlRetorno ?? Url.Content("~/");
            var mensaje = "";
            string email = "";

            if (remoteError is not null)
            {
                mensaje = $"Error del proveedor externo: {remoteError}";
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var info = await signInManager.GetExternalLoginInfoAsync();

            if (info is null)
            {
                mensaje = "Error cargando la data de login externo";
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var resultadoLoginExterno = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: false);

            // ya la cuenta existe
            if (resultadoLoginExterno.Succeeded) return LocalRedirect(urlRetorno);


            // el usuario no tiene una cuenta en la app

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email);
            }
            else
            {
                mensaje = $"Error leyendo el email del usuario del proveedor";
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            // instanciar IdentityUser
            var usuario = new IdentityUser { Email = email, UserName = email };

            var resultadoCrearUsuario = await userManager.CreateAsync(usuario);

            if (!resultadoCrearUsuario.Succeeded)
            {
                mensaje = resultadoCrearUsuario.Errors.First().Description;
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var resultadoAgregarLogin = await userManager.AddLoginAsync(usuario, info);


            if (resultadoAgregarLogin.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: true, info.LoginProvider);
                return LocalRedirect(urlRetorno);
            }

            mensaje = "Ha ocurrido un error agregando el login";
            return RedirectToAction("login", routeValues: new { mensaje });
        }



        // Administración de usuarios
        [HttpGet]
        [Authorize(Roles = Constantes.RolAdministrador)]
        public async Task<IActionResult> Listado(string mensaje = null)
        {
            var usuarios = await context.Users.Select(u => new UsuarioViewModel
            {
                Email = u.Email
            }).ToListAsync();

            var modelo = new UsuariosListadoViewModel();
            modelo.Usuarios = usuarios;
            modelo.Mensaje = mensaje;

            return View(modelo);
        }


        [HttpPost]
        [Authorize(Roles = Constantes.RolAdministrador)]
        public async Task<IActionResult> HacerAdmin(string email)
        {
            var usuario = await context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            if (usuario is null) return NotFound();

            await userManager.AddToRoleAsync(usuario, Constantes.RolAdministrador);

            return RedirectToAction("Listado", routeValues: new { mensaje = $"Rol Asignado Correctamente a {email}" });
        }


        [HttpPost]
        [Authorize(Roles = Constantes.RolAdministrador)]
        public async Task<IActionResult> RemoverAdmin(string email)
        {
            var usuario = await context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            if (usuario is null) return NotFound();

            await userManager.RemoveFromRoleAsync(usuario, Constantes.RolAdministrador);

            return RedirectToAction("Listado", routeValues: new { mensaje = $"Rol Removido Correctamente a {email}" });
        }

    }
}
