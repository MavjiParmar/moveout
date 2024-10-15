using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoxnMove.Database.Migrations
{
    /// <inheritdoc />
    public partial class updateContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "ProjectFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContactType",
                table: "Contacts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFiles_ContactId",
                table: "ProjectFiles",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFiles_Contacts_ContactId",
                table: "ProjectFiles",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFiles_Contacts_ContactId",
                table: "ProjectFiles");

            migrationBuilder.DropIndex(
                name: "IX_ProjectFiles_ContactId",
                table: "ProjectFiles");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "ProjectFiles");

            migrationBuilder.DropColumn(
                name: "ContactType",
                table: "Contacts");
        }
    }
}
