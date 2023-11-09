using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_FDark.Data.Migrations
{
    public partial class ajoutContentPicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Content",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Content");
        }
    }
}
