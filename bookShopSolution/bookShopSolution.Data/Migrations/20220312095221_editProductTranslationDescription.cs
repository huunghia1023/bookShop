using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookShopSolution.Data.Migrations
{
    public partial class editProductTranslationDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "ProductTranslations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ProductTranslations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "ProductTranslations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ProductTranslations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a768150-5de9-48d0-97df-9d1542314334"),
                column: "ConcurrencyStamp",
                value: "3bc11c77-e647-4c2c-9576-1741d4dc6644");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("7297be39-5977-40af-9aaf-4b57b21b24c1"),
                column: "ConcurrencyStamp",
                value: "48feacca-48cc-4c89-9109-f9c0ea7e19bf");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("5f9ec3c0-6f07-4103-ab0a-413f961c8b06"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bd4bb996-fa0e-46b2-813b-afd89bf05d11", "AQAAAAEAACcQAAAAECrc9pNx3SjZz6Lcfc9isAKdqnZ7VVQY2pgvf1qZifY/F9NnKZ6GcRmeOtp6VWZSfw==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bab47f6a-ca90-4fc2-a18d-484060a1332b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "719faf16-9959-4196-bc34-c3637a3040bf", "AQAAAAEAACcQAAAAEGVSoqNUlj23eK5HA4CMFlT2nyVPriVPfWgsY3YgatVqIvoOk+kOkTKqVEiIFNVHeQ==" });
        }
    }
}
