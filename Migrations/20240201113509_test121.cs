using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomPageApp.Migrations
{
    /// <inheritdoc />
    public partial class test121 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_registerVm",
                table: "registerVm");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "registerVm",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "registerVm",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_registerVm",
                table: "registerVm",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_registerVm",
                table: "registerVm");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "registerVm");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "registerVm",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_registerVm",
                table: "registerVm",
                column: "Name");
        }
    }
}
