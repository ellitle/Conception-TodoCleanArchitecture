using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanTodo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPirateShips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PirateShips",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    captain = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    goldCargo = table.Column<decimal>(type: "decimal(35,28)", precision: 35, scale: 28, nullable: false),
                    crewSize = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: false, defaultValue: "docked")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    createdAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP(6)"),
                    lastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "UTC_TIMESTAMP(6)"),
                    createdBy = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PirateShips", x => x.id);
                    table.CheckConstraint("CK_PirateShips_Captain", "CHAR_LENGTH(TRIM(`captain`)) BETWEEN 2 AND 50");
                    table.CheckConstraint("CK_PirateShips_CreatedBy", "CHAR_LENGTH(TRIM(`createdBy`)) > 0");
                    table.CheckConstraint("CK_PirateShips_CrewSize", "`crewSize` BETWEEN 1 AND 500");
                    table.CheckConstraint("CK_PirateShips_GoldCargo", "`goldCargo` BETWEEN 0 AND 1000000");
                    table.CheckConstraint("CK_PirateShips_LastModified", "`lastModified` >= `createdAt`");
                    table.CheckConstraint("CK_PirateShips_Name", "CHAR_LENGTH(TRIM(`name`)) BETWEEN 2 AND 100");
                    table.CheckConstraint("CK_PirateShips_Status", "BINARY `status` IN ('docked', 'sailing')");
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PirateShips");
        }
    }
}
