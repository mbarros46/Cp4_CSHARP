using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace c.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PATIO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Endereco = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Capacidade = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    OcupacaoAtual = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATIO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MOTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Modelo = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Placa = table.Column<string>(type: "NCHAR(7)", fixedLength: true, maxLength: 7, nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    Ano = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PatioId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MOTO_PATIO_PatioId",
                        column: x => x.PatioId,
                        principalTable: "PATIO",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MOTO_PatioId",
                table: "MOTO",
                column: "PatioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOTO");

            migrationBuilder.DropTable(
                name: "PATIO");
        }
    }
}
