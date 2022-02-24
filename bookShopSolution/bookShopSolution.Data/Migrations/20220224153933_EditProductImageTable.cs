using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookShopSolution.Data.Migrations
{
    public partial class EditProductImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 22, 39, 33, 152, DateTimeKind.Local).AddTicks(4718),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(6870));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Today",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 22, 39, 33, 152, DateTimeKind.Local).AddTicks(2963),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(6048));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDay",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 22, 39, 33, 152, DateTimeKind.Local).AddTicks(2699),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(5911));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "ProductImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 22, 39, 33, 151, DateTimeKind.Local).AddTicks(6135),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(3064));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 22, 39, 33, 151, DateTimeKind.Local).AddTicks(543),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(129));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a768150-5de9-48d0-97df-9d1542314334"),
                column: "ConcurrencyStamp",
                value: "35b6576b-11ea-4ff2-8a29-d9716bae3e84");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bab47f6a-ca90-4fc2-a18d-484060a1332b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c3c8a41c-2127-472d-b30c-1c2be0b2622e", "AQAAAAEAACcQAAAAEGbv+CwOfSlajVmz5XlWmKtA5oeQW8b6iGBgXTMvk2jn/qWV5RieBiSRqSwEQLrKCw==" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(6870),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 22, 39, 33, 152, DateTimeKind.Local).AddTicks(4718));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Today",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(6048),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 22, 39, 33, 152, DateTimeKind.Local).AddTicks(2963));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDay",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(5911),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 22, 39, 33, 152, DateTimeKind.Local).AddTicks(2699));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "ProductImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(3064),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 22, 39, 33, 151, DateTimeKind.Local).AddTicks(6135));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 14, 43, 50, 55, DateTimeKind.Local).AddTicks(129),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 22, 39, 33, 151, DateTimeKind.Local).AddTicks(543));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a768150-5de9-48d0-97df-9d1542314334"),
                column: "ConcurrencyStamp",
                value: "01eb2354-aabc-42c4-a50e-75979b893f36");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bab47f6a-ca90-4fc2-a18d-484060a1332b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "042df80c-6543-47d7-9e8e-149b56399928", "AQAAAAEAACcQAAAAECfGzQ2evpSvmaSeVjjwrj4NdhaSdJuM1NCVH4oztMIWoIvbCb6AgPU0y7eqGn6btw==" });
        }
    }
}
