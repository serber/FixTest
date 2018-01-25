using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace FixTest.Migrations
{
    public partial class AddWebSiteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WEB_SITE",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CHECK_INTERVAL = table.Column<long>(nullable: false),
                    IS_AVAILABLE = table.Column<bool>(nullable: true),
                    LAST_CHECKED_AT = table.Column<DateTimeOffset>(nullable: true),
                    URL = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WEB_SITE", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "WEB_SITE");
        }
    }
}