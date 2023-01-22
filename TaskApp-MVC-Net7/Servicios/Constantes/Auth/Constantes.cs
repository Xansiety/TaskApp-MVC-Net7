using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskApp.Servicios.Constantes.Auth
{
    public class Constantes
    {
        public const string RolAdministrador = "admin";

        public static readonly SelectListItem[] CulturasUISoportadas = new SelectListItem[]
        {
            new SelectListItem { Value = "es", Text = "Español" },
            new SelectListItem { Value = "en", Text = "English" }
        };
    }
}
