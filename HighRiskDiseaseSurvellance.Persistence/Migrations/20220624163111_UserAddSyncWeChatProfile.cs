using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HighRiskDiseaseSurvellance.Persistence.Migrations
{
    public partial class UserAddSyncWeChatProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasSyncWeChatUserProfile",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSyncWeChatUserProfile",
                table: "Users");
        }
    }
}
