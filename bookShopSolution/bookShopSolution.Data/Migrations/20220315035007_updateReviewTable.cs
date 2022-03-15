using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookShopSolution.Data.Migrations
{
    public partial class updateReviewTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_AppUsers_AppUserId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Products_ProductId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_AppUserId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Review");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ProductId",
                table: "Reviews",
                newName: "IX_Reviews_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a768150-5de9-48d0-97df-9d1542314334"),
                column: "ConcurrencyStamp",
                value: "cc0deaa4-cdd7-4ef0-99bf-0aa1d685b220");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("7297be39-5977-40af-9aaf-4b57b21b24c1"),
                column: "ConcurrencyStamp",
                value: "068d1037-0ec7-494b-9917-3aa3fdc02ffa");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("5f9ec3c0-6f07-4103-ab0a-413f961c8b06"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "defe46b4-d35f-4a64-9549-13d9d94f7083", "AQAAAAEAACcQAAAAEP/v2rK56YiYiWkv9fFieL1NAcONIcbboyIRua9JgC2XSgKn01GuNa2T1Ywov3UUMA==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bab47f6a-ca90-4fc2-a18d-484060a1332b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "60fa14f4-c0b8-4500-8bbf-ee7be9ab6cf7", "AQAAAAEAACcQAAAAENE1XZHyN1YzU9XMRdQjOFZv6k3/jiXoLHlx+4gUyjM/gLjzr6wcS5d5lUfGflnxbA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AppUsers_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AppUsers_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ProductId",
                table: "Review",
                newName: "IX_Review_ProductId");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Review",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a768150-5de9-48d0-97df-9d1542314334"),
                column: "ConcurrencyStamp",
                value: "11a7992a-2cd4-435e-9731-fc8df5d79398");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("7297be39-5977-40af-9aaf-4b57b21b24c1"),
                column: "ConcurrencyStamp",
                value: "1c6bd67a-1839-4ec5-9326-b1cf3b135d93");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("5f9ec3c0-6f07-4103-ab0a-413f961c8b06"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "987bb12b-9358-47c5-9ed8-a510576acc05", "AQAAAAEAACcQAAAAEKFoEsqRQZH1HMM9Ebv9/ENpaxOdPahKGV+xNQ8DQv2v3vlJxg62vAhCXXp2Zm09Qw==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bab47f6a-ca90-4fc2-a18d-484060a1332b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "82f794a0-4593-4bc9-86bf-998c3fd84971", "AQAAAAEAACcQAAAAEMNaFbu9VNJxBoXgLjwQz1TYKnCzvxLTfaS2HcxI6R2L97codHBrimk9ryTBX3qkSw==" });

            migrationBuilder.CreateIndex(
                name: "IX_Review_AppUserId",
                table: "Review",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AppUsers_AppUserId",
                table: "Review",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Products_ProductId",
                table: "Review",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
