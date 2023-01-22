using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskAppMVCNet7.Migrations
{
    /// <inheritdoc />
    public partial class AdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Añadir el rol de administrador si no existe
            migrationBuilder.Sql(@"if NOT EXISTS(SELECT ID FROM AspNetRoles WHERE Id = 'fc3db4a9-0e3c-4f5c-8d51-9eb266cd7db3')
                                BEGIN
                                    insert AspNetRoles(id, [name], [NormalizedName])
                                    VALUES('fc3db4a9-0e3c-4f5c-8d51-9eb266cd7db3', 'admin', 'ADMIN')
                                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete AspNetRoles where Id = 'fc3db4a9-0e3c-4f5c-8d51-9eb266cd7db3'");
        }
    }
}
