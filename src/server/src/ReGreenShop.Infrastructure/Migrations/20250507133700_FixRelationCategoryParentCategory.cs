using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReGreenShop.Infrastructure.Migrations;

/// <inheritdoc />
public partial class FixRelationCategoryParentCategory : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Categories_Categories_ParentCategoryId",
            table: "Categories");

        migrationBuilder.DropIndex(
            name: "IX_Categories_ParentCategoryId",
            table: "Categories");

        migrationBuilder.CreateIndex(
            name: "IX_Categories_ParentCategoryId",
            table: "Categories",
            column: "ParentCategoryId");

        migrationBuilder.AddForeignKey(
            name: "FK_Categories_Categories_ParentCategoryId",
            table: "Categories",
            column: "ParentCategoryId",
            principalTable: "Categories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Categories_Categories_ParentCategoryId",
            table: "Categories");

        migrationBuilder.DropIndex(
            name: "IX_Categories_ParentCategoryId",
            table: "Categories");

        migrationBuilder.CreateIndex(
            name: "IX_Categories_ParentCategoryId",
            table: "Categories",
            column: "ParentCategoryId",
            unique: true,
            filter: "[ParentCategoryId] IS NOT NULL");

        migrationBuilder.AddForeignKey(
            name: "FK_Categories_Categories_ParentCategoryId",
            table: "Categories",
            column: "ParentCategoryId",
            principalTable: "Categories",
            principalColumn: "Id");
    }
}
