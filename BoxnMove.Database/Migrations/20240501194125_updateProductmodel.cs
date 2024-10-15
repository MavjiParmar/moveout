using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoxnMove.Database.Migrations
{
    /// <inheritdoc />
    public partial class updateProductmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CFT",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "BuildType",
                table: "ProductTypes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CFT",
                table: "ProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "ProductTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildType",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "CFT",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "ProductTypes");

            migrationBuilder.AddColumn<string>(
                name: "BuildType",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CFT",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
