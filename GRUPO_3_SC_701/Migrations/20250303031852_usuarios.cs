using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GRUPO_3_SC_701.Migrations
{
    /// <inheritdoc />
    public partial class usuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3b3516a5-3285-4ded-9e02-d313b6c4805f", "AQAAAAIAAYagAAAAEDUYPm2cob02tE2mlN5m4cCD6SToo5oO1Q+CnHsyZQx5mGxhXQzAsd5QjorUl0NBJA==", "92c40702-1b7f-4e0e-93f4-86c0deee5709" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1002", 0, "08ae753a-8126-4c16-b170-88c12c9fce40", "user@domain.com", true, false, null, "USER@DOMAIN.COM", "USER@DOMAIN.COM", "AQAAAAIAAYagAAAAEPOG1fjlY1LtbXjAK0XI9SDE8CHVue7EtzL3Q2o3OweYGdkkL7w64+sfb2eaOynBsQ==", null, false, "04999c39-5cd1-4482-a85d-ef6aed6f2305", false, "user@domain.com" },
                    { "1003", 0, "8ed20187-e6e9-45ab-ac3f-5701608c8fdc", "client@domain.com", true, false, null, "CLIENT@DOMAIN.COM", "CLIENT@DOMAIN.COM", "AQAAAAIAAYagAAAAELQHhjAZAv+p/vTXKIY8r4G9Z/LGusz1XOrymW+FgF0WeK06p6TEmqOBZksGjABhFw==", null, false, "f7d122a6-75b2-43d5-b18c-94b46759e0f0", false, "client@domain.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "2", "1002" },
                    { "3", "1003" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "1002" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "1003" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1002");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1003");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "90b031ef-c887-4741-941f-ae830a4ab022", "AQAAAAIAAYagAAAAEMSYYjIflLVFRHmcLAQTqfFYosrSUMozBpPCzHqWZTlhev6YZLvEaL6hx39nVKkCAA==", "d91a3887-6af9-47bc-9bf5-d9f6a9446509" });
        }
    }
}
