using Microsoft.EntityFrameworkCore;
using TaskApp_MVC_Net7.Entidades;

namespace TaskApp_MVC_Net7
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        // Api Fluent -- configurar entidades, pero no realiza las validaciones
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Tarea>().Property(x => x.Titulo).IsRequired().HasMaxLength(250);
        }

        // configurar entidades
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Paso> Pasos { get; set; }
        public DbSet<ArchivoAjunto> ArchivoAjuntos { get; set; }
    }
}
