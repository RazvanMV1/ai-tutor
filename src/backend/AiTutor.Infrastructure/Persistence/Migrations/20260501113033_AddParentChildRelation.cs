using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiTutor.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddParentChildRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvitationCode",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_InvitationCode",
                table: "Users",
                column: "InvitationCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_InvitationCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InvitationCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Users");
        }
    }
}
