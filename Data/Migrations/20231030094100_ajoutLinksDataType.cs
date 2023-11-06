using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_FDark.Data.Migrations
{
    public partial class ajoutLinksDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataType",
                table: "Links",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataType",
                table: "Links");
        }
    }
}
