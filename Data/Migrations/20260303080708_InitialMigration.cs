using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetwebapi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Picture = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Role = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true, defaultValue: "ACTIVE"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CC4C70D55250", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Users__536C85E482BB4FEC",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
