using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateSoftDeleteEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "Product",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleteBy",
                table: "Product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleteByUserId",
                table: "Product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "Category",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleteBy",
                table: "Category",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleteByUserId",
                table: "Category",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Product_DeleteByUserId",
                table: "Product",
                column: "DeleteByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_DeleteByUserId",
                table: "Category",
                column: "DeleteByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_User_DeleteByUserId",
                table: "Category",
                column: "DeleteByUserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_User_DeleteByUserId",
                table: "Product",
                column: "DeleteByUserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_User_DeleteByUserId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_User_DeleteByUserId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_DeleteByUserId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Category_DeleteByUserId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DeleteByUserId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DeleteByUserId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Category");
        }
    }
}
