using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10261874_PROG6212_POE.Migrations
{
    /// <inheritdoc />
    public partial class migOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomFileNames",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "SupportingDocumentFileName",
                table: "Claims");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomFileNames",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SupportingDocumentFileName",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
