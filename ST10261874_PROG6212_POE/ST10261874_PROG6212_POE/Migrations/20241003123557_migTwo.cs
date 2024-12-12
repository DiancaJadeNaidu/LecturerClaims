using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10261874_PROG6212_POE.Migrations
{
    /// <inheritdoc />
    public partial class migTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SupportingDocumentFileNames",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportingDocumentFileNames",
                table: "Claims");
        }
    }
}
