using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courier_Data_Control_App.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryCustomerName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Deliveries SET CustomerName = '' WHERE CustomerName IS NULL");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "Deliveries",
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Deliveries",
                type: "TEXT",
                nullable: false, 
                defaultValue: "Entrega Estándar", 
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "Deliveries",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Deliveries",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
