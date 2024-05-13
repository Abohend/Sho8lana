using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace src.Migrations
{
    /// <inheritdoc />
    public partial class ProposalAndReplay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectsProposal_AspNetUsers_FreelancerId",
                table: "ProjectsProposal");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectsProposal_Projects_ProjectId",
                table: "ProjectsProposal");

            migrationBuilder.DropTable(
                name: "JobsProposal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectsProposal",
                table: "ProjectsProposal");

            migrationBuilder.DropColumn(
                name: "ClientNote",
                table: "ProjectsProposal");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "ProjectsProposal");

            migrationBuilder.RenameTable(
                name: "ProjectsProposal",
                newName: "Proposal");

            migrationBuilder.RenameColumn(
                name: "OfferedPrice",
                table: "Proposal",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "OfferedDeliverDate",
                table: "Proposal",
                newName: "DeliverDate");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectsProposal_ProjectId",
                table: "Proposal",
                newName: "IX_Proposal_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectsProposal_FreelancerId",
                table: "Proposal",
                newName: "IX_Proposal_FreelancerId");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Proposal",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Proposal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Proposal",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "Proposal",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Proposal",
                table: "Proposal",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProposalReplay",
                columns: table => new
                {
                    ProposalId = table.Column<int>(type: "int", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalReplay", x => x.ProposalId);
                    table.ForeignKey(
                        name: "FK_ProposalReplay_Proposal_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_JobId",
                table: "Proposal",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proposal_AspNetUsers_FreelancerId",
                table: "Proposal",
                column: "FreelancerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Jobs_JobId",
                table: "Proposal",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);   

            migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Projects_ProjectId",
                table: "Proposal",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposal_AspNetUsers_FreelancerId",
                table: "Proposal");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposal_Jobs_JobId",
                table: "Proposal");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposal_Projects_ProjectId",
                table: "Proposal");

            migrationBuilder.DropTable(
                name: "ProposalReplay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Proposal",
                table: "Proposal");

            migrationBuilder.DropIndex(
                name: "IX_Proposal_JobId",
                table: "Proposal");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Proposal");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Proposal");

            migrationBuilder.RenameTable(
                name: "Proposal",
                newName: "ProjectsProposal");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ProjectsProposal",
                newName: "OfferedPrice");

            migrationBuilder.RenameColumn(
                name: "DeliverDate",
                table: "ProjectsProposal",
                newName: "OfferedDeliverDate");

            migrationBuilder.RenameIndex(
                name: "IX_Proposal_ProjectId",
                table: "ProjectsProposal",
                newName: "IX_ProjectsProposal_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Proposal_FreelancerId",
                table: "ProjectsProposal",
                newName: "IX_ProjectsProposal_FreelancerId");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "ProjectsProposal",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ProjectsProposal",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ClientNote",
                table: "ProjectsProposal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "ProjectsProposal",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectsProposal",
                table: "ProjectsProposal",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "JobsProposal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FreelancerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliverDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobsProposal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobsProposal_AspNetUsers_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_JobsProposal_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobsProposal_FreelancerId",
                table: "JobsProposal",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_JobsProposal_JobId",
                table: "JobsProposal",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectsProposal_AspNetUsers_FreelancerId",
                table: "ProjectsProposal",
                column: "FreelancerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectsProposal_Projects_ProjectId",
                table: "ProjectsProposal",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
