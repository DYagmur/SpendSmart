using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpendSmart.Migrations
{
    /// <inheritdoc />
    public partial class AddImageSizesToExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Expenses");

            migrationBuilder.AddColumn<string>(
                name: "LargeImagePath",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MediumImagePath",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SmallImagePath",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LargeImagePath",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "MediumImagePath",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "SmallImagePath",
                table: "Expenses");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
