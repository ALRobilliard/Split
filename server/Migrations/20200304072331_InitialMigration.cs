using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace split.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:categorytype", "income,expense")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", "'uuid-ossp', '', ''");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userId = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    firstName = table.Column<string>(type: "character varying(50)", nullable: true),
                    lastName = table.Column<string>(type: "character varying(50)", nullable: true),
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
                    table.PrimaryKey("PK_User", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    accountId = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    accountName = table.Column<string>(type: "character varying(20)", nullable: false),
                    userId = table.Column<Guid>(nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    accountType = table.Column<int>(nullable: false),
                    balance = table.Column<decimal>(type: "money", nullable: true),
                    limit = table.Column<decimal>(type: "money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.accountId);
                    table.ForeignKey(
                        name: "Account_userId_fkey",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    categoryId = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    categoryName = table.Column<string>(type: "character varying(25)", nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    userId = table.Column<Guid>(nullable: false),
                    categoryType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.categoryId);
                    table.ForeignKey(
                        name: "Category_userId_fkey",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContact",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "character varying(30)", nullable: true),
                    contactId = table.Column<Guid>(nullable: true),
                    userId = table.Column<Guid>(nullable: true),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContact", x => x.id);
                    table.ForeignKey(
                        name: "UserContact_contactId_fkey",
                        column: x => x.contactId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "UserContact_userId_fkey",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionParty",
                columns: table => new
                {
                    transactionPartyId = table.Column<Guid>(nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    transactionPartyName = table.Column<string>(type: "character varying(25)", nullable: false),
                    defaultCategoryId = table.Column<Guid>(nullable: true),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    modifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    userId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionParty", x => x.transactionPartyId);
                    table.ForeignKey(
                        name: "TransactionParty_defaultCategoryId_fkey",
                        column: x => x.defaultCategoryId,
                        principalTable: "Category",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "TransactionParty_userId_fkey",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
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
                    table.PrimaryKey("PK_Transaction", x => x.transactionId);
                    table.ForeignKey(
                        name: "Transaction_accountInId_fkey",
                        column: x => x.accountInId,
                        principalTable: "Account",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Transaction_accountOutId_fkey",
                        column: x => x.accountOutId,
                        principalTable: "Account",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Transaction_categoryId_fkey",
                        column: x => x.categoryId,
                        principalTable: "Category",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Transaction_transactionPartyId_fkey",
                        column: x => x.transactionPartyId,
                        principalTable: "TransactionParty",
                        principalColumn: "transactionPartyId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "Transaction_userId_fkey",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SplitPayment",
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
                    table.PrimaryKey("PK_SplitPayment", x => x.splitPaymentId);
                    table.ForeignKey(
                        name: "SplitPayment_payeeId_fkey",
                        column: x => x.payeeId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "SplitPayment_transactionId_fkey",
                        column: x => x.transactionId,
                        principalTable: "Transaction",
                        principalColumn: "transactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_userId",
                table: "Account",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_userId",
                table: "Category",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitPayment_payeeId",
                table: "SplitPayment",
                column: "payeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitPayment_transactionId",
                table: "SplitPayment",
                column: "transactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_accountInId",
                table: "Transaction",
                column: "accountInId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_accountOutId",
                table: "Transaction",
                column: "accountOutId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_categoryId",
                table: "Transaction",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_transactionPartyId",
                table: "Transaction",
                column: "transactionPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_userId",
                table: "Transaction",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionParty_defaultCategoryId",
                table: "TransactionParty",
                column: "defaultCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionParty_userId",
                table: "TransactionParty",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "User_email_key",
                table: "User",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserContact_contactId",
                table: "UserContact",
                column: "contactId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContact_userId",
                table: "UserContact",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SplitPayment");

            migrationBuilder.DropTable(
                name: "UserContact");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "TransactionParty");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
