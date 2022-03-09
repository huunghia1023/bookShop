using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookShopSolution.Data.Migrations
{
    public partial class seedCustomerRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 3, 8, 15, 0, 53, 638, DateTimeKind.Local).AddTicks(1228),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 3, 5, 22, 31, 15, 245, DateTimeKind.Local).AddTicks(7955));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a768150-5de9-48d0-97df-9d1542314334"),
                column: "ConcurrencyStamp",
                value: "df7c5614-0753-44f2-8642-4fd78aea661c");

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("7297be39-5977-40af-9aaf-4b57b21b24c1"), "3ca67596-4d03-4b65-be39-e3a21cbf663f", "Customer role", "customer", "customer" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bab47f6a-ca90-4fc2-a18d-484060a1332b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f70c30d8-e3b8-4161-bb79-d70edb7f5532", "AQAAAAEAACcQAAAAEFN91n04EOaFMuIHFZ21vwzLhgE/BIcLPctyFnq+tRB1F1x50todycuZYVEW8tEszw==", "" });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDay", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("5f9ec3c0-6f07-4103-ab0a-413f961c8b06"), 0, new DateTime(1999, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "52a0e180-7e58-4dce-ba08-43148a5bf9d6", "nghiadev@gmail.com", true, "Nghia", "Dev", false, null, "nghiadev@gmail.com", "customer", "AQAAAAEAACcQAAAAEIReOlAJ3nPSdQlZUNrq1QSU88UDHA9UP7XIYUA5GsPpxavWh7qEcEAlUL2jX4aWuQ==", null, false, "", false, "customer" });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("7297be39-5977-40af-9aaf-4b57b21b24c1"), new Guid("5f9ec3c0-6f07-4103-ab0a-413f961c8b06") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("7297be39-5977-40af-9aaf-4b57b21b24c1"), new Guid("5f9ec3c0-6f07-4103-ab0a-413f961c8b06") });

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("7297be39-5977-40af-9aaf-4b57b21b24c1"));

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("5f9ec3c0-6f07-4103-ab0a-413f961c8b06"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 3, 5, 22, 31, 15, 245, DateTimeKind.Local).AddTicks(7955),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 3, 8, 15, 0, 53, 638, DateTimeKind.Local).AddTicks(1228));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a768150-5de9-48d0-97df-9d1542314334"),
                column: "ConcurrencyStamp",
                value: "93a3b6f2-743b-45fa-985c-a1d30c29909c");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bab47f6a-ca90-4fc2-a18d-484060a1332b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0e88ab3b-35ef-4a27-9d0e-a6138406cf5c", "AQAAAAEAACcQAAAAEACtSSSzxQGV9tHn/P5romG0g5MRrKBhmAaMXXiaeAI4+ti9eVv6FF1QxBrTlBXCpQ==", null });
        }
    }
}
