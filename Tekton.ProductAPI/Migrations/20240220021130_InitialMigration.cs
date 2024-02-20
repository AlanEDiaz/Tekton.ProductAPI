using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tekton.ProductAPI.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CommandStore",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandStore", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Stock = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "CommandStore",
                columns: new[] { "id", "CreatedAt", "Data", "ProductId", "Type" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 19, 23, 11, 30, 496, DateTimeKind.Local).AddTicks(3191), "test", new Guid("f69eb360-b6a2-450f-97ce-c8914e028bfe"), "Create" },
                    { 2, new DateTime(2024, 2, 19, 23, 11, 30, 496, DateTimeKind.Local).AddTicks(3518), "test", new Guid("431be9fe-9063-40d6-9d21-be654bd7613f"), "Update" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CreatedOn", "Description", "Discount", "Name", "Price", "Stock", "UpdatedOn" },
                values: new object[,]
                {
                    { new Guid("f31cc5fa-bcd7-462e-92c5-10e0f78d246a"), new DateTime(2024, 2, 19, 23, 11, 30, 493, DateTimeKind.Local).AddTicks(8381), "test product1", 0m, "ProductSeed1", 234m, 12, null },
                    { new Guid("afdf6038-3d39-443f-9ec5-6378dc5dac45"), new DateTime(2024, 2, 19, 23, 11, 30, 494, DateTimeKind.Local).AddTicks(8718), "test product2", 0m, "ProductSeed2", 23m, 14, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandStore");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
