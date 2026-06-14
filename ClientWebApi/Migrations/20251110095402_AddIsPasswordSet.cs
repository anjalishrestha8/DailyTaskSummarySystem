using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPasswordSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPasswordSet",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPasswordSet",
                table: "AspNetUsers");
        }
    }
}
