using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courier_Data_Control_App.Migrations
{
    /// <inheritdoc />
    public partial class ImagePathDriverTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Drivers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Drivers");
        }
    }
}
