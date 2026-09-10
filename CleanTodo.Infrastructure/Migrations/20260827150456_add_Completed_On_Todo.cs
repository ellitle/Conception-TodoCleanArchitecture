using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanTodo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_Completed_On_Todo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "User",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "User",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "uuid(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Todos",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompleted",
                table: "Todos",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Todos",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "uuid(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompetedOn",
                table: "Todos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompetedOn",
                table: "Todos");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "User",
                type: "uuid(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Todos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<ulong>(
                name: "IsCompleted",
                table: "Todos",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Todos",
                type: "uuid(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");
        }
    }
}
