using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookShopSolution.Data.Migrations
{
    public partial class SeedIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(6870),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 13, 56, 23, 55, DateTimeKind.Local).AddTicks(8216));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Today",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(6048),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 13, 56, 23, 55, DateTimeKind.Local).AddTicks(7164));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDay",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(5911),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 13, 56, 23, 55, DateTimeKind.Local).AddTicks(7026));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "ProductImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(3064),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 13, 56, 23, 55, DateTimeKind.Local).AddTicks(4060));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(129),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 13, 56, 23, 55, DateTimeKind.Local).AddTicks(983));

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("6a768150-5de9-48d0-97df-9d1542314334"), "01eb2354-aabc-42c4-a50e-75979b893f36", "Adminstrator role", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("6a768150-5de9-48d0-97df-9d1542314334"), new Guid("bab47f6a-ca90-4fc2-a18d-484060a1332b") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDay", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("bab47f6a-ca90-4fc2-a18d-484060a1332b"), 0, new DateTime(1999, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "042df80c-6543-47d7-9e8e-149b56399928", "nguyengiahuunghia118@gmail.com", true, "Nghia", "Gia", false, null, "nguyengiahuunghia118@gmail.com", "admin", "AQAAAAEAACcQAAAAECfGzQ2evpSvmaSeVjjwrj4NdhaSdJuM1NCVH4oztMIWoIvbCb6AgPU0y7eqGn6btw==", null, false, null, false, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a768150-5de9-48d0-97df-9d1542314334"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("6a768150-5de9-48d0-97df-9d1542314334"), new Guid("bab47f6a-ca90-4fc2-a18d-484060a1332b") });

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bab47f6a-ca90-4fc2-a18d-484060a1332b"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 13, 56, 23, 55, DateTimeKind.Local).AddTicks(8216),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(6870));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Today",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 13, 56, 23, 55, DateTimeKind.Local).AddTicks(7164),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(6048));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDay",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 13, 56, 23, 55, DateTimeKind.Local).AddTicks(7026),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(5911));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "ProductImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 13, 56, 23, 55, DateTimeKind.Local).AddTicks(4060),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(3064));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 13, 56, 23, 55, DateTimeKind.Local).AddTicks(983),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(129));
        }
    }
}
