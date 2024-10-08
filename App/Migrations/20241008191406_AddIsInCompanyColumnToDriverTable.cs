using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courier_Data_Control_App.Migrations
{
    /// <inheritdoc />
    public partial class AddIsInCompanyColumnToDriverTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInCompany",
                table: "Drivers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInCompany",
                table: "Drivers");
        }
    }
}
