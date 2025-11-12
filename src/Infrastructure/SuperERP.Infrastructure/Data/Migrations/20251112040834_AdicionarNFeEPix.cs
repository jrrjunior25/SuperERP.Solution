using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperERP.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarNFeEPix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NFes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Serie = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Modelo = table.Column<string>(type: "text", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    VendaId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataEmissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAutorizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValorProdutos = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorFrete = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorDesconto = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ChaveAcesso = table.Column<string>(type: "character varying(44)", maxLength: 44, nullable: true),
                    Protocolo = table.Column<string>(type: "text", nullable: true),
                    XmlNota = table.Column<string>(type: "text", nullable: true),
                    XmlRetorno = table.Column<string>(type: "text", nullable: true),
                    MotivoRejeicao = table.Column<string>(type: "text", nullable: true),
                    JustificativaCancelamento = table.Column<string>(type: "text", nullable: true),
                    DataCancelamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pix",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: true),
                    VendaId = table.Column<Guid>(type: "uuid", nullable: true),
                    TxId = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ChavePix = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    QRCode = table.Column<string>(type: "text", nullable: false),
                    QRCodeBase64 = table.Column<string>(type: "text", nullable: false),
                    PixCopiaECola = table.Column<string>(type: "text", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataExpiracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndToEndId = table.Column<string>(type: "text", nullable: true),
                    Pagador = table.Column<string>(type: "text", nullable: true),
                    PagadorCpfCnpj = table.Column<string>(type: "text", nullable: true),
                    InfoAdicional = table.Column<string>(type: "text", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pix", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemNFe",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NFeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    NCM = table.Column<string>(type: "text", nullable: false),
                    CFOP = table.Column<string>(type: "text", nullable: false),
                    Quantidade = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    BaseICMS = table.Column<decimal>(type: "numeric", nullable: false),
                    AliquotaICMS = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorICMS = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemNFe", x => new { x.NFeId, x.Id });
                    table.ForeignKey(
                        name: "FK_ItemNFe_NFes_NFeId",
                        column: x => x.NFeId,
                        principalTable: "NFes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemNFe");

            migrationBuilder.DropTable(
                name: "Pix");

            migrationBuilder.DropTable(
                name: "NFes");
        }
    }
}
