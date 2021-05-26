using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Backend5.Migrations
{
    public partial class AddRelationsBetweenPlacementAndPatientAndWard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WardPlacements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WardStaffs",
                table: "WardStaffs");

            migrationBuilder.DropColumn(
                name: "WardStaffId",
                table: "WardStaffs");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Diagnoses",
                newName: "Details");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "WardStaffs",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Placements",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WardId",
                table: "Placements",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WardStaffs",
                table: "WardStaffs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WardStaffs_WardId",
                table: "WardStaffs",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Placements_PatientId",
                table: "Placements",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Placements_WardId",
                table: "Placements",
                column: "WardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_Patients_PatientId",
                table: "Placements",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_Wards_WardId",
                table: "Placements",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Patients_PatientId",
                table: "Placements");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Wards_WardId",
                table: "Placements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WardStaffs",
                table: "WardStaffs");

            migrationBuilder.DropIndex(
                name: "IX_WardStaffs_WardId",
                table: "WardStaffs");

            migrationBuilder.DropIndex(
                name: "IX_Placements_PatientId",
                table: "Placements");

            migrationBuilder.DropIndex(
                name: "IX_Placements_WardId",
                table: "Placements");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WardStaffs");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Placements");

            migrationBuilder.DropColumn(
                name: "WardId",
                table: "Placements");

            migrationBuilder.RenameColumn(
                name: "Details",
                table: "Diagnoses",
                newName: "Status");

            migrationBuilder.AddColumn<int>(
                name: "WardStaffId",
                table: "WardStaffs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WardStaffs",
                table: "WardStaffs",
                columns: new[] { "WardId", "WardStaffId" });

            migrationBuilder.CreateTable(
                name: "WardPlacements",
                columns: table => new
                {
                    WardId = table.Column<int>(nullable: false),
                    PlacementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WardPlacements", x => new { x.WardId, x.PlacementId });
                    table.ForeignKey(
                        name: "FK_WardPlacements_Placements_PlacementId",
                        column: x => x.PlacementId,
                        principalTable: "Placements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WardPlacements_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WardPlacements_PlacementId",
                table: "WardPlacements",
                column: "PlacementId");
        }
    }
}
