using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandMadeStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Brand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ArabicName",
                table: "Products",
                newName: "arabicName");

            migrationBuilder.RenameColumn(
                name: "ArabicDescription",
                table: "Products",
                newName: "arabicDescription");

            migrationBuilder.RenameColumn(
                name: "ArabicName",
                table: "Categories",
                newName: "arabicName");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "arabicName",
                table: "Brands",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "arabicName",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "arabicName",
                table: "Products",
                newName: "ArabicName");

            migrationBuilder.RenameColumn(
                name: "arabicDescription",
                table: "Products",
                newName: "ArabicDescription");

            migrationBuilder.RenameColumn(
                name: "arabicName",
                table: "Categories",
                newName: "ArabicName");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
