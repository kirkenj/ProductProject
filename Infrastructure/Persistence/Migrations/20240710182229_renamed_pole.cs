using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class renamed_pole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0128a68d-0e6a-47bb-a312-4ea911146dbf"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("548dde6c-714e-4a6c-9fae-f2c99e2ee312"));

            migrationBuilder.RenameColumn(
                name: "HasgAlgorithm",
                table: "Users",
                newName: "HashAlgorithm");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Email", "HashAlgorithm", "IsEmailConfirmed", "Login", "PasswordHash", "RoleID", "StringEncoding" },
                values: new object[,]
                {
                    { new Guid("68f6d052-a320-4f27-9950-373b175b29e8"), "Confidential", null, "MD5", false, "user", "�˱�R����#�", 2, "Unicode (UTF-8)" },
                    { new Guid("edd3adc8-3f65-4dd2-acfd-1a2e0add186a"), "Confidential", null, "MD5", false, "admin", "!#/)zW��C�JJ��", 1, "Unicode (UTF-8)" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("68f6d052-a320-4f27-9950-373b175b29e8"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("edd3adc8-3f65-4dd2-acfd-1a2e0add186a"));

            migrationBuilder.RenameColumn(
                name: "HashAlgorithm",
                table: "Users",
                newName: "HasgAlgorithm");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Email", "HasgAlgorithm", "IsEmailConfirmed", "Login", "PasswordHash", "RoleID", "StringEncoding" },
                values: new object[,]
                {
                    { new Guid("0128a68d-0e6a-47bb-a312-4ea911146dbf"), "Confidential", null, "MD5", false, "admin", "!#/)zW��C�JJ��", 1, "Unicode (UTF-8)" },
                    { new Guid("548dde6c-714e-4a6c-9fae-f2c99e2ee312"), "Confidential", null, "MD5", false, "user", "�˱�R����#�", 2, "Unicode (UTF-8)" }
                });
        }
    }
}
