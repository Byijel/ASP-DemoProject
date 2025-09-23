using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ap.Demo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "City");

            migrationBuilder.EnsureSchema(
                name: "Country");

            migrationBuilder.CreateTable(
                name: "tblCountries",
                schema: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCountries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblCities",
                schema: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Population = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblCities_tblCountries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "Country",
                        principalTable: "tblCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Country",
                table: "tblCountries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "België" },
                    { 2, "Nederland" },
                    { 3, "Frankrijk" }
                });

            migrationBuilder.InsertData(
                schema: "City",
                table: "tblCities",
                columns: new[] { "Id", "CountryId", "Name", "Population" },
                values: new object[,]
                {
                    { 1, 1, "Antwerpen", 530000 },
                    { 2, 1, "Gent", 260000 },
                    { 3, 2, "Amsterdam", 820000 },
                    { 4, 3, "Parijs", 2140000 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblCities_CountryId",
                schema: "City",
                table: "tblCities",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblCities",
                schema: "City");

            migrationBuilder.DropTable(
                name: "tblCountries",
                schema: "Country");
        }
    }
}
