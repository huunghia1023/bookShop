using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookShopSolution.Data.Migrations
{
    public partial class editOrderAndOrderDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderDetails",
                newName: "TotalPrice");

            migrationBuilder.AlterColumn<string>(
                name: "ShipAddress",
                table: "Orders",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a768150-5de9-48d0-97df-9d1542314334"),
                column: "ConcurrencyStamp",
                value: "6742fb1e-6c11-48e8-8c72-fb9e31d8b309");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("7297be39-5977-40af-9aaf-4b57b21b24c1"),
                column: "ConcurrencyStamp",
                value: "3985d9b6-2a2a-412e-a467-f7984d8a8533");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("5f9ec3c0-6f07-4103-ab0a-413f961c8b06"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cc5fe682-33e5-48f7-9892-7a4ce6fc1077", "AQAAAAEAACcQAAAAEGhY1l89OdUHspfnMDc6u1STIwmFZhz/t1e2zKBxtXQHwse40H/8Ej39eMosAeP1XA==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bab47f6a-ca90-4fc2-a18d-484060a1332b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2ddf69e6-3928-4156-9b64-1d8a1aaa3e57", "AQAAAAEAACcQAAAAECSrbS2NOG8cc2ryoDntAmxcZZayip6uXQnhazIC4XouoQ96DLdZC0DrYBtwiwwqBg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "OrderDetails",
                newName: "Price");

            migrationBuilder.AlterColumn<string>(
                name: "ShipAddress",
                table: "Orders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a768150-5de9-48d0-97df-9d1542314334"),
                column: "ConcurrencyStamp",
                value: "af2931ad-7f88-448a-b299-a7c92e13471a");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("7297be39-5977-40af-9aaf-4b57b21b24c1"),
                column: "ConcurrencyStamp",
                value: "b82fc1af-7d6c-46a2-9ea9-1d50a24855ab");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("5f9ec3c0-6f07-4103-ab0a-413f961c8b06"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e4ad3f66-1506-4c09-ae75-684bda330397", "AQAAAAEAACcQAAAAELHMziBLauhmXtnmbyyTyZDk97yYVQyV2tgz8CVkPIHP1JsJ2v96Zhn37kX/xZ2LiA==" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bab47f6a-ca90-4fc2-a18d-484060a1332b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "76e785e6-bf5a-47aa-aa0d-915a6a91b4cc", "AQAAAAEAACcQAAAAEN/17PdVvrFLzk2i/CpZTE/iO0wCbIklkRkzEWcWiNjMOs9/bWLiljQOviwAEUVMkA==" });
        }
    }
}
