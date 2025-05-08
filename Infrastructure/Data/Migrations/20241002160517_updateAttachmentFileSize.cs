using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateAttachmentFileSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "Attachments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62f94fe7-0580-42df-969c-50d4165d63b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4288a431-0def-4869-8c5b-48a231722cfa", "AQAAAAIAAYagAAAAEEMPnfXqEz0lBmO/YipL3fAzdX/kcGuSGwwrdzihn349o1xCgEW5FtNsp7I67XLMeA==", "db1813b1-935f-47e1-999a-c13abf86b81a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FileSize",
                table: "Attachments",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "62f94fe7-0580-42df-969c-50d4165d63b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "03442c8f-363a-416a-b8d9-015ba921ad20", "AQAAAAIAAYagAAAAELFQ429AE0YKNBTnmWamrj8rMt68QfFDm0pb0dt34h9Ql0SnUCPJbDSMSLDH1J9bQQ==", "4085adc7-a9c2-4518-9322-19c2083dfd22" });
        }
    }
}
