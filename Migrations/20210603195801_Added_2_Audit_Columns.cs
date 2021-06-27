using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStoreApplication.Migrations
{
    public partial class Added_2_Audit_Columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Books");
        }
    }
}
