using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sho8lana.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Chat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "chat");

            migrationBuilder.CreateTable(
                name: "GroupChats",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupChats_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OnlineUsers",
                schema: "chat",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConnectionId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    ReceiverUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReceiverGroupId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_ReceiverUserId",
                        column: x => x.ReceiverUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_GroupChats_ReceiverGroupId",
                        column: x => x.ReceiverGroupId,
                        principalSchema: "chat",
                        principalTable: "GroupChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupChats",
                columns: table => new
                {
                    ChatsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MembersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupChats", x => new { x.ChatsId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_UserGroupChats_AspNetUsers_MembersId",
                        column: x => x.MembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroupChats_GroupChats_ChatsId",
                        column: x => x.ChatsId,
                        principalSchema: "chat",
                        principalTable: "GroupChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChats_AdminId",
                schema: "chat",
                table: "GroupChats",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverGroupId",
                schema: "chat",
                table: "Messages",
                column: "ReceiverGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverUserId",
                schema: "chat",
                table: "Messages",
                column: "ReceiverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                schema: "chat",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupChats_MembersId",
                table: "UserGroupChats",
                column: "MembersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "OnlineUsers",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "UserGroupChats");

            migrationBuilder.DropTable(
                name: "GroupChats",
                schema: "chat");
        }
    }
}
