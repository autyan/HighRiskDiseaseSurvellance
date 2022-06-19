using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HighRiskDiseaseSurvellance.Persistence.Migrations
{
    public partial class UpdateUserFieldLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DistributorQrCode",
                table: "Users",
                type: "longtext",
                maxLength: 20000,
                nullable: true,
                collation: "utf8_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(1024)",
                oldMaxLength: 1024,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("Relational:Collation", "utf8_general_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DistributorQrCode",
                table: "Users",
                type: "varchar(1024)",
                maxLength: 1024,
                nullable: true,
                collation: "utf8_general_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldMaxLength: 20000,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("Relational:Collation", "utf8_general_ci");
        }
    }
}
