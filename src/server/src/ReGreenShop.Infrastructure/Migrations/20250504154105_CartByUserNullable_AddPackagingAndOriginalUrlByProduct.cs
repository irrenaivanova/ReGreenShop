using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReGreenShop.Infrastructure.Migrations;

/// <inheritdoc />
public partial class CartByUserNullable_AddPackagingAndOriginalUrlByProduct : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_AspNetUsers_Carts_CartId",
            table: "AspNetUsers");

        migrationBuilder.DropIndex(
            name: "IX_AspNetUsers_CartId",
            table: "AspNetUsers");

        migrationBuilder.AddColumn<string>(
            name: "OriginalUrl",
            table: "Products",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Packaging",
            table: "Products",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "CartId",
            table: "AspNetUsers",
            type: "nvarchar(450)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUsers_CartId",
            table: "AspNetUsers",
            column: "CartId",
            unique: true,
            filter: "[CartId] IS NOT NULL");

        migrationBuilder.AddForeignKey(
            name: "FK_AspNetUsers_Carts_CartId",
            table: "AspNetUsers",
            column: "CartId",
            principalTable: "Carts",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_AspNetUsers_Carts_CartId",
            table: "AspNetUsers");

        migrationBuilder.DropIndex(
            name: "IX_AspNetUsers_CartId",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "OriginalUrl",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "Packaging",
            table: "Products");

        migrationBuilder.AlterColumn<string>(
            name: "CartId",
            table: "AspNetUsers",
            type: "nvarchar(450)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(450)",
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUsers_CartId",
            table: "AspNetUsers",
            column: "CartId",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_AspNetUsers_Carts_CartId",
            table: "AspNetUsers",
            column: "CartId",
            principalTable: "Carts",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
