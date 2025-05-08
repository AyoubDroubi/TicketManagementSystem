using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class addroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "48395b9b-a292-4a48-80df-ebc6a057f6c0", null, "Admin", "ADMIN" },
                    { "6367a40d-3014-43bc-a704-135f41abae2d", null, "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62f94fe7-0580-42df-969c-50d4165d63b9",
                columns: new[] { "ConcurrencyStamp", "IsDeleted", "PasswordHash", "SecurityStamp" },
                values: new object[] { "27b99dce-ba5a-4b6e-8dde-6e1f826a50d0", false, "AQAAAAIAAYagAAAAEIfhKNcHfzfS+YTxvxQVwSAKEX7yKM+4cSL8A0S3ojna2lY9URKxMygKsAR5ASzrcQ==", "c2f0954c-720d-4d31-b750-575b8fc6f32b" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "48395b9b-a292-4a48-80df-ebc6a057f6c0", "62f94fe7-0580-42df-969c-50d4165d63b9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6367a40d-3014-43bc-a704-135f41abae2d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "48395b9b-a292-4a48-80df-ebc6a057f6c0", "62f94fe7-0580-42df-969c-50d4165d63b9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48395b9b-a292-4a48-80df-ebc6a057f6c0");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62f94fe7-0580-42df-969c-50d4165d63b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4288a431-0def-4869-8c5b-48a231722cfa", "AQAAAAIAAYagAAAAEEMPnfXqEz0lBmO/YipL3fAzdX/kcGuSGwwrdzihn349o1xCgEW5FtNsp7I67XLMeA==", "db1813b1-935f-47e1-999a-c13abf86b81a" });
        }
    }
}
