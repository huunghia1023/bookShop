using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookShopSolution.Data.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 1, 24, 5, 66, DateTimeKind.Local).AddTicks(1738),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 23, 22, 4, 3, 572, DateTimeKind.Local).AddTicks(2975));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Today",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 1, 24, 5, 65, DateTimeKind.Local).AddTicks(3302),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 23, 22, 4, 3, 571, DateTimeKind.Local).AddTicks(4916));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDay",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 1, 24, 5, 65, DateTimeKind.Local).AddTicks(3155),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 23, 22, 4, 3, 571, DateTimeKind.Local).AddTicks(4764));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "ProductImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 1, 24, 5, 65, DateTimeKind.Local).AddTicks(124),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 23, 22, 4, 3, 571, DateTimeKind.Local).AddTicks(1715));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 24, 1, 24, 5, 64, DateTimeKind.Local).AddTicks(8917),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 23, 22, 4, 3, 571, DateTimeKind.Local).AddTicks(502));

            migrationBuilder.InsertData(
                table: "AppConfigs",
                columns: new[] { "Key", "Value" },
                values: new object[,]
                {
                    { "HomeDescription", "This is description of bookShopSolution" },
                    { "HomeKeyword", "This is keyword of bookShopSolution" },
                    { "HomeTitle", "This is home page of bookShopSolution" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "IsShowOnHome", "ParentId", "SortOrder", "Status" },
                values: new object[,]
                {
                    { 1, true, null, 0, 1 },
                    { 2, true, null, 0, 1 }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "LanguageId", "IsDefault", "LanguageCOD", "LanguageName" },
                values: new object[] { 1, true, "vi-VN", "Tiếng Việt" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "LanguageId", "LanguageCOD", "LanguageName" },
                values: new object[] { 2, "en-US", "English" });

            migrationBuilder.InsertData(
                table: "CategoryTranslations",
                columns: new[] { "CategoryTranslationId", "CategoryId", "CategoryName", "LanguageId", "SeoAlias", "SeoDescription", "SeoTitle" },
                values: new object[,]
                {
                    { 1, 1, "Văn Học", 1, "van-hoc", "Sách thể loại văn học", "Sách văn học" },
                    { 2, 1, "Literary", 2, "literary", "literary", "Literary Book" },
                    { 3, 2, "Kinh Tế", 1, "kinh-te", "Sách thuộc thể loại kinh tế", "Sách kinh tế" },
                    { 4, 2, "Economic", 2, "economic", "economic", "Economic Book" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Key",
                keyValue: "HomeDescription");

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Key",
                keyValue: "HomeKeyword");

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Key",
                keyValue: "HomeTitle");

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "CategoryTranslationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "CategoryTranslationId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "CategoryTranslationId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CategoryTranslations",
                keyColumn: "CategoryTranslationId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "LanguageId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "LanguageId",
                keyValue: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 23, 22, 4, 3, 572, DateTimeKind.Local).AddTicks(2975),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 1, 24, 5, 66, DateTimeKind.Local).AddTicks(1738));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Today",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 23, 22, 4, 3, 571, DateTimeKind.Local).AddTicks(4916),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 1, 24, 5, 65, DateTimeKind.Local).AddTicks(3302));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDay",
                table: "Promotions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 23, 22, 4, 3, 571, DateTimeKind.Local).AddTicks(4764),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 1, 24, 5, 65, DateTimeKind.Local).AddTicks(3155));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "ProductImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 23, 22, 4, 3, 571, DateTimeKind.Local).AddTicks(1715),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 1, 24, 5, 65, DateTimeKind.Local).AddTicks(124));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 23, 22, 4, 3, 571, DateTimeKind.Local).AddTicks(502),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 24, 1, 24, 5, 64, DateTimeKind.Local).AddTicks(8917));
        }
    }
}
