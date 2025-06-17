using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReGreenShop.Infrastructure.Migrations;

/// <inheritdoc />
public partial class NullableBaseCategoryIdinOrderDetails : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_OrderDetails_Categories_BaseCategoryId",
            table: "OrderDetails");

        migrationBuilder.AlterColumn<int>(
            name: "BaseCategoryId",
            table: "OrderDetails",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddForeignKey(
            name: "FK_OrderDetails_Categories_BaseCategoryId",
            table: "OrderDetails",
            column: "BaseCategoryId",
            principalTable: "Categories",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_OrderDetails_Categories_BaseCategoryId",
            table: "OrderDetails");

        migrationBuilder.AlterColumn<int>(
            name: "BaseCategoryId",
            table: "OrderDetails",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_OrderDetails_Categories_BaseCategoryId",
            table: "OrderDetails",
            column: "BaseCategoryId",
            principalTable: "Categories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
