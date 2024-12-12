using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentType",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentType", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StandartArea = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FacilityCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EquipmentTypeCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Contract_EquipmentType_EquipmentTypeCode",
                        column: x => x.EquipmentTypeCode,
                        principalTable: "EquipmentType",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contract_Facility_FacilityCode",
                        column: x => x.FacilityCode,
                        principalTable: "Facility",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EquipmentType",
                columns: new[] { "Code", "Area", "Name" },
                values: new object[,]
                {
                    { "123456c", 1, "ToolBox" },
                    { "123456d", 50, "Vehicle" }
                });

            migrationBuilder.InsertData(
                table: "Facility",
                columns: new[] { "Code", "Name", "StandartArea" },
                values: new object[,]
                {
                    { "123456a", "Facility1", 100 },
                    { "123456b", "Facility2", 200 }
                });

            migrationBuilder.InsertData(
                table: "Contract",
                columns: new[] { "Code", "Amount", "EquipmentTypeCode", "FacilityCode" },
                values: new object[] { "123456z", 2, "123456d", "123456a" });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_EquipmentTypeCode",
                table: "Contract",
                column: "EquipmentTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_FacilityCode",
                table: "Contract",
                column: "FacilityCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "EquipmentType");

            migrationBuilder.DropTable(
                name: "Facility");
        }
    }
}
