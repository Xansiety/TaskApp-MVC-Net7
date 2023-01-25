using System.Security.Claims;

namespace TaskApp.Servicios
{
    public interface IServicioUsuarios
    {
        string ObtenerUsuarioId();
    }

    public class ServicioUsuarios : IServicioUsuarios
    {
        private readonly HttpContext httpContext;

        public ServicioUsuarios(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContext = httpContextAccessor.HttpContext;
        }

        public string ObtenerUsuarioId()
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

                return idClaim.Value;
            }
            else
            {
                throw new Exception("El usuario no está autenticado");
            }

        }
    }
}
