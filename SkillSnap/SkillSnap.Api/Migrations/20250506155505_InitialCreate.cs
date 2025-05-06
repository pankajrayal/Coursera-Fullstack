using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillSnap.Api.Migrations {
  /// <inheritdoc />
  public partial class InitialCreate: Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
          name: "PortfolioUsers",
          columns: table => new {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
            ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_PortfolioUsers", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Projects",
          columns: table => new {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
            ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
            PortfolioUserId = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_Projects", x => x.Id);
            table.ForeignKey(
                      name: "FK_Projects_PortfolioUsers_PortfolioUserId",
                      column: x => x.PortfolioUserId,
                      principalTable: "PortfolioUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Skills",
          columns: table => new {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
            PortfolioUserId = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_Skills", x => x.Id);
            table.ForeignKey(
                      name: "FK_Skills_PortfolioUsers_PortfolioUserId",
                      column: x => x.PortfolioUserId,
                      principalTable: "PortfolioUsers",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_Projects_PortfolioUserId",
          table: "Projects",
          column: "PortfolioUserId");

      migrationBuilder.CreateIndex(
          name: "IX_Skills_PortfolioUserId",
          table: "Skills",
          column: "PortfolioUserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "Projects");

      migrationBuilder.DropTable(
          name: "Skills");

      migrationBuilder.DropTable(
          name: "PortfolioUsers");
    }
  }
}