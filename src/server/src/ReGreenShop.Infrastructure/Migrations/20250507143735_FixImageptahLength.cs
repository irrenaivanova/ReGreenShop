﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReGreenShop.Infrastructure.Migrations;

/// <inheritdoc />
public partial class FixImageptahLength : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "LocalPath",
            table: "Images",
            type: "nvarchar(200)",
            maxLength: 200,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(100)",
            oldMaxLength: 100,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "BlobPath",
            table: "Images",
            type: "nvarchar(200)",
            maxLength: 200,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(100)",
            oldMaxLength: 100,
            oldNullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "LocalPath",
            table: "Images",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(200)",
            oldMaxLength: 200,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "BlobPath",
            table: "Images",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(200)",
            oldMaxLength: 200,
            oldNullable: true);
    }
}
