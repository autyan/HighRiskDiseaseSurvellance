using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HighRiskDiseaseSurvellance.Persistence.Migrations
{
    public partial class RecordAddScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Score",
                table: "Records",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "Records");
        }
    }
}
