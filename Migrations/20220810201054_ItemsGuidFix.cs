using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DailyHelper.Migrations
{
    public partial class ItemsGuidFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ListId",
                table: "ShopItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListId",
                table: "ShopItem");
        }
    }
}
