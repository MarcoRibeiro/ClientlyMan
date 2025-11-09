using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientlyMan.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Customers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                TaxNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                Phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Customers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Policies",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                Insurer = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                ProductType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                PolicyNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Premium = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                RenewNotifyDays = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Policies", x => x.Id);
                table.ForeignKey(
                    name: "FK_Policies_Customers_CustomerId",
                    column: x => x.CustomerId,
                    principalTable: "Customers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Simulations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                RequestedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Simulations", x => x.Id);
                table.ForeignKey(
                    name: "FK_Simulations_Customers_CustomerId",
                    column: x => x.CustomerId,
                    principalTable: "Customers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Customers_TaxNumber",
            table: "Customers",
            column: "TaxNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Policies_CustomerId",
            table: "Policies",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_Policies_PolicyNumber",
            table: "Policies",
            column: "PolicyNumber");

        migrationBuilder.CreateIndex(
            name: "IX_Simulations_CustomerId",
            table: "Simulations",
            column: "CustomerId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Policies");

        migrationBuilder.DropTable(
            name: "Simulations");

        migrationBuilder.DropTable(
            name: "Customers");
    }
}
