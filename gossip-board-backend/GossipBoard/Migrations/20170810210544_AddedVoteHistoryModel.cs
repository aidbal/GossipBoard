using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GossipBoard.Migrations
{
    public partial class AddedVoteHistoryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoteHistoryId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VoteHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_VoteHistoryId",
                table: "AspNetUsers",
                column: "VoteHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_VoteHistories_VoteHistoryId",
                table: "AspNetUsers",
                column: "VoteHistoryId",
                principalTable: "VoteHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_VoteHistories_VoteHistoryId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "VoteHistories");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_VoteHistoryId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VoteHistoryId",
                table: "AspNetUsers");
        }
    }
}
