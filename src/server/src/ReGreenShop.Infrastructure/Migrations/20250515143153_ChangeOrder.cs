using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReGreenShop.Infrastructure.Migrations;

/// <inheritdoc />
public partial class ChangeOrder : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Orders_DeliveryPrices_DeliveryPriceId",
            table: "Orders");

        migrationBuilder.AlterColumn<decimal>(
            name: "TotalPrice",
            table: "Orders",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<int>(
            name: "DeliveryPriceId",
            table: "Orders",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddColumn<decimal>(
            name: "TotalPrice",
            table: "OrderDetails",
            type: "decimal(18,2)",
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddForeignKey(
            name: "FK_Orders_DeliveryPrices_DeliveryPriceId",
            table: "Orders",
            column: "DeliveryPriceId",
            principalTable: "DeliveryPrices",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Orders_DeliveryPrices_DeliveryPriceId",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "TotalPrice",
            table: "OrderDetails");

        migrationBuilder.AlterColumn<string>(
            name: "TotalPrice",
            table: "Orders",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");

        migrationBuilder.AlterColumn<int>(
            name: "DeliveryPriceId",
            table: "Orders",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Orders_DeliveryPrices_DeliveryPriceId",
            table: "Orders",
            column: "DeliveryPriceId",
            principalTable: "DeliveryPrices",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
