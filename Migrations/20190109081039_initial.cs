using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace splitapi.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    categoryType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Transaction_Party",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    defaultCategory = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction_Party", x => x.id);
                    table.ForeignKey(
                        name: "Transaction_Party_defaultCategory_fkey",
                        column: x => x.defaultCategory,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    category = table.Column<Guid>(nullable: true),
                    transactionParty = table.Column<Guid>(nullable: true),
                    amount = table.Column<decimal>(type: "money", nullable: false),
                    isShared = table.Column<bool>(nullable: false),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    accountOut = table.Column<Guid>(nullable: true),
                    accountIn = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.id);
                    table.ForeignKey(
                        name: "Transaction_accountIn_fkey",
                        column: x => x.accountIn,
                        principalTable: "Accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Transaction_accountOut_fkey",
                        column: x => x.accountOut,
                        principalTable: "Accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Transaction_category_fkey",
                        column: x => x.category,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Transaction_transactionParty_fkey",
                        column: x => x.transactionParty,
                        principalTable: "Transaction_Party",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Party_defaultCategory",
                table: "Transaction_Party",
                column: "defaultCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_accountIn",
                table: "Transactions",
                column: "accountIn");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_accountOut",
                table: "Transactions",
                column: "accountOut");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_category",
                table: "Transactions",
                column: "category");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_transactionParty",
                table: "Transactions",
                column: "transactionParty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Transaction_Party");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
