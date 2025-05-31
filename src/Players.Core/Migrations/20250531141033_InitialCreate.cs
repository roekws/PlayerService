using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Players.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Health = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Hero = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: false),
                    Health = table.Column<int>(type: "integer", nullable: false),
                    Gold = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DotaId = table.Column<long>(type: "bigint", nullable: false),
                    SteamId = table.Column<long>(type: "bigint", nullable: false),
                    PublicName = table.Column<string>(type: "text", nullable: false),
                    IsPublicForLadder = table.Column<bool>(type: "boolean", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CurrentMatchId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Structures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BaseId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: false),
                    Health = table.Column<int>(type: "integer", nullable: false),
                    GridX = table.Column<int>(type: "integer", nullable: false),
                    GridY = table.Column<int>(type: "integer", nullable: false),
                    GridZ = table.Column<int>(type: "integer", nullable: false),
                    Rotation = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Structures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Structures_Bases_BaseId",
                        column: x => x.BaseId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterAbilities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterAbilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterAbilities_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterItems_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false),
                    BaseId = table.Column<long>(type: "bigint", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    RatingChange = table.Column<int>(type: "integer", nullable: false),
                    GameClientVersion = table.Column<int>(type: "integer", nullable: false),
                    Result = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Bases_BaseId",
                        column: x => x.BaseId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StructureAbilities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StructureId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StructureAbilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StructureAbilities_Structures_StructureId",
                        column: x => x.StructureId,
                        principalTable: "Structures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchBattles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MatchId = table.Column<long>(type: "bigint", nullable: false),
                    EnemyPlayerId = table.Column<long>(type: "bigint", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CharacterHealthChange = table.Column<int>(type: "integer", nullable: false),
                    BaseHealthChange = table.Column<int>(type: "integer", nullable: false),
                    GoldChange = table.Column<int>(type: "integer", nullable: false),
                    ExperienceChange = table.Column<int>(type: "integer", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    EnemyCharacterSnapshotJson = table.Column<string>(type: "jsonb", nullable: true),
                    EnemyBaseSnapshotJson = table.Column<string>(type: "jsonb", nullable: true),
                    PlayerCharacterSnapshotJson = table.Column<string>(type: "jsonb", nullable: true),
                    PlayerBaseSnapshotJson = table.Column<string>(type: "jsonb", nullable: true),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchBattles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchBattles_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchBattles_Players_EnemyPlayerId",
                        column: x => x.EnemyPlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterAbilities_CharacterId",
                table: "CharacterAbilities",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterItems_CharacterId",
                table: "CharacterItems",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchBattles_EnemyPlayerId",
                table: "MatchBattles",
                column: "EnemyPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchBattles_MatchId",
                table: "MatchBattles",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_BaseId",
                table: "Matches",
                column: "BaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_CharacterId",
                table: "Matches",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Level",
                table: "Matches",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_PlayerId",
                table: "Matches",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CurrentMatchId",
                table: "Players",
                column: "CurrentMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_DotaId",
                table: "Players",
                column: "DotaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_SteamId",
                table: "Players",
                column: "SteamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StructureAbilities_StructureId",
                table: "StructureAbilities",
                column: "StructureId");

            migrationBuilder.CreateIndex(
                name: "IX_Structures_BaseId",
                table: "Structures",
                column: "BaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterAbilities");

            migrationBuilder.DropTable(
                name: "CharacterItems");

            migrationBuilder.DropTable(
                name: "MatchBattles");

            migrationBuilder.DropTable(
                name: "StructureAbilities");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Structures");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Bases");
        }
    }
}
