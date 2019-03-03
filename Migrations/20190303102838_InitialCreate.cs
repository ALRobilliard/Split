using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace splitapi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    firstName = table.Column<string>(type: "character varying(50)", nullable: true),
                    lastName = table.Column<string>(type: "character varying(50)", nullable: true),
                    username = table.Column<string>(type: "character varying(20)", nullable: false),
                    email = table.Column<string>(type: "character varying(50)", nullable: false),
                    isRegistered = table.Column<bool>(nullable: true, defaultValueSql: "true"),
                    confirmedEmail = table.Column<bool>(nullable: true, defaultValueSql: "false"),
                    passwordHash = table.Column<byte[]>(nullable: true),
                    passwordSalt = table.Column<byte[]>(nullable: true),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    lastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    accountId = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    accountName = table.Column<string>(type: "character varying(20)", nullable: false),
                    userId = table.Column<Guid>(nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.accountId);
                    table.ForeignKey(
                        name: "Account_userId_fkey",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    categoryId = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    categoryName = table.Column<string>(type: "character varying(25)", nullable: false),
                    categoryType = table.Column<string>(type: "character varying(10)", nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    userId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.categoryId);
                    table.ForeignKey(
                        name: "Category_userId_fkey",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionParties",
                columns: table => new
                {
                    transactionPartyId = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    transactionPartyName = table.Column<string>(type: "character varying(25)", nullable: false),
                    defaultCategoryId = table.Column<Guid>(nullable: true),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionParties", x => x.transactionPartyId);
                    table.ForeignKey(
                        name: "TransactionParty_defaultCategoryId_fkey",
                        column: x => x.defaultCategoryId,
                        principalTable: "Categories",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    transactionId = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    categoryId = table.Column<Guid>(nullable: true),
                    transactionPartyId = table.Column<Guid>(nullable: true),
                    accountInId = table.Column<Guid>(nullable: true),
                    accountOutId = table.Column<Guid>(nullable: true),
                    amount = table.Column<decimal>(type: "money", nullable: true),
                    isShared = table.Column<bool>(nullable: false),
                    transactionDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    userId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.transactionId);
                    table.ForeignKey(
                        name: "Transaction_accountInId_fkey",
                        column: x => x.accountInId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Transaction_accountOutId_fkey",
                        column: x => x.accountOutId,
                        principalTable: "Accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Transaction_categoryId_fkey",
                        column: x => x.categoryId,
                        principalTable: "Categories",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Transaction_transactionPartyId_fkey",
                        column: x => x.transactionPartyId,
                        principalTable: "TransactionParties",
                        principalColumn: "transactionPartyId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Transaction_userId_fkey",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SplitPayments",
                columns: table => new
                {
                    splitPaymentId = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    transactionId = table.Column<Guid>(nullable: false),
                    payeeId = table.Column<Guid>(nullable: false),
                    amount = table.Column<decimal>(type: "money", nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SplitPayments", x => x.splitPaymentId);
                    table.ForeignKey(
                        name: "SplitPayment_payeeId_fkey",
                        column: x => x.payeeId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "SplitPayment_transactionId_fkey",
                        column: x => x.transactionId,
                        principalTable: "Transactions",
                        principalColumn: "transactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_userId",
                table: "Accounts",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_userId",
                table: "Categories",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitPayments_payeeId",
                table: "SplitPayments",
                column: "payeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitPayments_transactionId",
                table: "SplitPayments",
                column: "transactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionParties_defaultCategoryId",
                table: "TransactionParties",
                column: "defaultCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_accountInId",
                table: "Transactions",
                column: "accountInId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_accountOutId",
                table: "Transactions",
                column: "accountOutId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_categoryId",
                table: "Transactions",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_transactionPartyId",
                table: "Transactions",
                column: "transactionPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_userId",
                table: "Transactions",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "User_email_key",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "User_username_key",
                table: "Users",
                column: "username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SplitPayments");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "TransactionParties");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
