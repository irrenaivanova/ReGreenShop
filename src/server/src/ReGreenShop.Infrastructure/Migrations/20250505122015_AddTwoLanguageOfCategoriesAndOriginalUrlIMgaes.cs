using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReGreenShop.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddTwoLanguageOfCategoriesAndOriginalUrlIMgaes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Name",
            table: "Categories");

        migrationBuilder.AddColumn<string>(
            name: "OriginalUrl",
            table: "Images",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "NameInBulgarian",
            table: "Categories",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "NameInEnglish",
            table: "Categories",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "OriginalUrl",
            table: "Images");

        migrationBuilder.DropColumn(
            name: "NameInBulgarian",
            table: "Categories");

        migrationBuilder.DropColumn(
            name: "NameInEnglish",
            table: "Categories");

        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "Categories",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "");
    }
}
