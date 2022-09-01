using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Observability.Service.A.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataItems", x => x.Id);
                });

            migrationBuilder.Sql("SET IDENTITY_INSERT DataItems ON");
            migrationBuilder.Sql("INSERT INTO DataItems (Id, Value) VALUES (1, 'value 1')");
            migrationBuilder.Sql("INSERT INTO DataItems (Id, Value) VALUES (2, 'value 2')");
            migrationBuilder.Sql("INSERT INTO DataItems (Id, Value) VALUES (3, 'value 3')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataItems");
        }
    }
}
