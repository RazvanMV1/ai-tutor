using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiTutor.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStripeFieldsToSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastStripeEventId",
                table: "Subscriptions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "Subscriptions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePriceId",
                table: "Subscriptions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeSubscriptionId",
                table: "Subscriptions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            // Backfill: subscription-urile existente seed-uite cu IsActive=true
            // sunt subscription-uri "legacy demo" și trebuie marcate ca Active (=2).
            // Cele cu IsActive=false (dacă există) rămân Canceled (=4).
            migrationBuilder.Sql(@"
                UPDATE ""Subscriptions""
                SET ""Status"" = 2
                WHERE ""IsActive"" = true;
            ");

            migrationBuilder.Sql(@"
                UPDATE ""Subscriptions""
                SET ""Status"" = 4
                WHERE ""IsActive"" = false;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_StripeCustomerId",
                table: "Subscriptions",
                column: "StripeCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_StripeSubscriptionId",
                table: "Subscriptions",
                column: "StripeSubscriptionId",
                unique: true,
                filter: "\"StripeSubscriptionId\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_StripeCustomerId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_StripeSubscriptionId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "LastStripeEventId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "StripePriceId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "StripeSubscriptionId",
                table: "Subscriptions");
        }
    }
}
