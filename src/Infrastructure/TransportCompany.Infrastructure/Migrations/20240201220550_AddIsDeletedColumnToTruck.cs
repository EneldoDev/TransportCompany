using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportCompany.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedColumnToTruck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Trucks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Trucks");
        }
    }
}
