using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DigitalSignature.Domain.Core.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client_Envelope_Information",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClientAuthToken = table.Column<string>(nullable: false),
                    EnvelopeId = table.Column<string>(nullable: false),
                    RecipientEmail = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    SentOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    UpdateOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    ReturnUrl = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client_Envelope_Information", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client_Envelope_Information");
        }
    }
}
