using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_FDark.Data.Migrations
{
    public partial class modificationMCD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "SubCatIds",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "SubCatId",
                table: "Links",
                newName: "ContentId");

            migrationBuilder.AddColumn<int>(
                name: "ContentTypeId",
                table: "SubCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExtensionId",
                table: "SubCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropColumn(
                name: "ContentTypeId",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "ExtensionId",
                table: "SubCategories");

            migrationBuilder.RenameColumn(
                name: "ContentId",
                table: "Links",
                newName: "SubCatId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Links",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SubCatIds",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
