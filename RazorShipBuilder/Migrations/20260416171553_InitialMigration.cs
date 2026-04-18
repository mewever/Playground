using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RazorShipBuilder.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ssbuild");

            migrationBuilder.CreateTable(
                name: "MountDevices",
                schema: "ssbuild",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequiresTurret = table.Column<bool>(type: "bit", nullable: false),
                    IsContinualUse = table.Column<bool>(type: "bit", nullable: false),
                    ReloadDuration = table.Column<int>(type: "int", nullable: false),
                    PowerDraw = table.Column<int>(type: "int", nullable: false),
                    TractorPower = table.Column<int>(type: "int", nullable: false),
                    EnergyDamage = table.Column<int>(type: "int", nullable: false),
                    PhysicalDamage = table.Column<int>(type: "int", nullable: false),
                    EnergyShield = table.Column<int>(type: "int", nullable: false),
                    PhysicalShield = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MountDevices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Starships",
                schema: "ssbuild",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GeneratorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PowerGeneration = table.Column<int>(type: "int", nullable: false),
                    MaximumPower = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Starships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MountPoints",
                schema: "ssbuild",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StarshipId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MountedDeviceId = table.Column<int>(type: "int", nullable: true),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    IsTurret = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MountPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MountPoints_MountDevices_MountedDeviceId",
                        column: x => x.MountedDeviceId,
                        principalSchema: "ssbuild",
                        principalTable: "MountDevices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MountPoints_Starships_StarshipId",
                        column: x => x.StarshipId,
                        principalSchema: "ssbuild",
                        principalTable: "Starships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MountPoints_MountedDeviceId",
                schema: "ssbuild",
                table: "MountPoints",
                column: "MountedDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_MountPoints_StarshipId",
                schema: "ssbuild",
                table: "MountPoints",
                column: "StarshipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MountPoints",
                schema: "ssbuild");

            migrationBuilder.DropTable(
                name: "MountDevices",
                schema: "ssbuild");

            migrationBuilder.DropTable(
                name: "Starships",
                schema: "ssbuild");
        }
    }
}
