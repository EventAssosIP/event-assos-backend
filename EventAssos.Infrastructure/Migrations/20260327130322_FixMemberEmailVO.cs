using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventAssos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixMemberEmailVO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddressValue",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddressValue",
                table: "Members");
        }
    }
}
