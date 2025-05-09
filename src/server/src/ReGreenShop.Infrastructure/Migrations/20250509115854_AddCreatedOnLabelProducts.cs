using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReGreenShop.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddCreatedOnLabelProducts : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<double>(
            name: "Duration",
            table: "LabelProducts",
            type: "float",
            nullable: false,
            defaultValue: 0.0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedOn",
            table: "LabelProducts",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "ModifiedOn",
            table: "LabelProducts",
            type: "datetime2",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CreatedOn",
            table: "LabelProducts");

        migrationBuilder.DropColumn(
            name: "ModifiedOn",
            table: "LabelProducts");

        migrationBuilder.AlterColumn<int>(
            name: "Duration",
            table: "LabelProducts",
            type: "int",
            nullable: true,
            oldClrType: typeof(double),
            oldType: "float");
    }
}
