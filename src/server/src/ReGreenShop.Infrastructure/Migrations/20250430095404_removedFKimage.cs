using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReGreenShop.Infrastructure.Migrations;

/// <inheritdoc />
public partial class removedFKimage : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Images_Categories_CategoryId",
            table: "Images");

        migrationBuilder.DropIndex(
            name: "IX_Images_CategoryId",
            table: "Images");

        migrationBuilder.DropColumn(
            name: "CategoryId",
            table: "Images");

        migrationBuilder.DropColumn(
            name: "ProductId",
            table: "Images");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "AspNetUserTokens",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(128)",
            oldMaxLength: 128);

        migrationBuilder.AlterColumn<string>(
            name: "LoginProvider",
            table: "AspNetUserTokens",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(128)",
            oldMaxLength: 128);

        migrationBuilder.AlterColumn<string>(
            name: "ProviderKey",
            table: "AspNetUserLogins",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(128)",
            oldMaxLength: 128);

        migrationBuilder.AlterColumn<string>(
            name: "LoginProvider",
            table: "AspNetUserLogins",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(128)",
            oldMaxLength: 128);

        migrationBuilder.CreateIndex(
            name: "IX_Categories_ImageId",
            table: "Categories",
            column: "ImageId",
            unique: true,
            filter: "[ImageId] IS NOT NULL");

        migrationBuilder.AddForeignKey(
            name: "FK_Categories_Images_ImageId",
            table: "Categories",
            column: "ImageId",
            principalTable: "Images",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Categories_Images_ImageId",
            table: "Categories");

        migrationBuilder.DropIndex(
            name: "IX_Categories_ImageId",
            table: "Categories");

        migrationBuilder.AddColumn<int>(
            name: "CategoryId",
            table: "Images",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "ProductId",
            table: "Images",
            type: "int",
            nullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "AspNetUserTokens",
            type: "nvarchar(128)",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AlterColumn<string>(
            name: "LoginProvider",
            table: "AspNetUserTokens",
            type: "nvarchar(128)",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AlterColumn<string>(
            name: "ProviderKey",
            table: "AspNetUserLogins",
            type: "nvarchar(128)",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AlterColumn<string>(
            name: "LoginProvider",
            table: "AspNetUserLogins",
            type: "nvarchar(128)",
            maxLength: 128,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.CreateIndex(
            name: "IX_Images_CategoryId",
            table: "Images",
            column: "CategoryId",
            unique: true,
            filter: "[CategoryId] IS NOT NULL");

        migrationBuilder.AddForeignKey(
            name: "FK_Images_Categories_CategoryId",
            table: "Images",
            column: "CategoryId",
            principalTable: "Categories",
            principalColumn: "Id");
    }
}
