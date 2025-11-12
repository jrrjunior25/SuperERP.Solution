# 🔄 Sincronização NFC-e com Banco de Dados

## 📋 Fluxo Completo

### 1. Venda no PDV → Emissão NFC-e → Banco de Dados

```
PDV → Finalizar Venda → Emitir NFC-e → SEFAZ → Salvar BD → Atualizar Frontend
```

## 🗄️ Estrutura do Banco

### Tabela NFes
```sql
CREATE TABLE "NFes" (
    "Id" uuid PRIMARY KEY,
    "Numero" varchar(20) NOT NULL,
    "Serie" varchar(10) NOT NULL,
    "Modelo" text NOT NULL,
    "EmpresaId" uuid NOT NULL,
    "ClienteId" uuid NOT NULL,
    "VendaId" uuid,
    "DataEmissao" timestamp NOT NULL,
    "DataAutorizacao" timestamp,
    "ValorTotal" numeric NOT NULL,
    "Status" text NOT NULL,
    "ChaveAcesso" varchar(44),
    "XmlNota" text,
    "XmlRetorno" text
);
```

### Tabela Pix
```sql
CREATE TABLE "Pix" (
    "Id" uuid PRIMARY KEY,
    "TxId" varchar(32) NOT NULL,
    "ChavePix" varchar(100) NOT NULL,
    "Valor" numeric NOT NULL,
    "Status" varchar(20) NOT NULL,
    "QRCode" text NOT NULL,
    "DataCriacao" timestamp NOT NULL,
    "DataExpiracao" timestamp NOT NULL,
    "VendaId" uuid
);
```

## 🔄 Processo de Sincronização

### 1. Finalizar Venda no PDV

```csharp
POST /api/vendas/{vendaId}/finalizar
{
  "empresaId": "guid",
  "formaPagamento": "PIX",
  "emitirNFCe": true
}
```

### 2. Handler Processa

```csharp
public async Task<FinalizarVendaPDVResult> Handle(...)
{
    // 1. Buscar venda
    var venda = await _vendaRepository.GetByIdAsync(vendaId);
    
    // 2. Finalizar venda
    venda.Finalizar();
    await _vendaRepository.UpdateAsync(venda);
    
    // 3. Emitir NFC-e
    var nfceResponse = await _nfeService.EmitirNFeAsync(request);
    
    // 4. Salvar NFC-e no banco
    if (nfceResponse.Sucesso)
    {
        var nfce = NFe.Criar(...);
        nfce.Autorizar(chaveAcesso, protocolo, xml);
        await _nfeRepository.AddAsync(nfce);
        
        // 5. Vincular à venda
        venda.VincularNFCe(chaveAcesso, xml);
        await _vendaRepository.UpdateAsync(venda);
    }
    
    return result;
}
```

### 3. Resposta ao PDV

```json
{
  "sucesso": true,
  "vendaFinalizada": true,
  "vendaId": "guid",
  "nfceId": "guid",
  "chaveNFCe": "35250112345678000190650010000000011234567890",
  "xmlNFCe": "<?xml...",
  "valorTotal": 150.00
}
```

## 📱 Frontend - Consulta NFC-e

### API Endpoints

```http
# Listar todas NFC-e
GET /api/nfce

# Obter por ID
GET /api/nfce/{id}

# Obter por chave de acesso
GET /api/nfce/chave/{chaveAcesso}

# Obter por venda
GET /api/nfce/venda/{vendaId}
```

### Página Blazor

```razor
@page "/nfce"

<h3>📄 NFC-e - Notas Fiscais</h3>

<table>
    @foreach (var nota in notas)
    {
        <tr>
            <td>@nota.Numero</td>
            <td>@nota.Status</td>
            <td>@nota.ChaveAcesso</td>
        </tr>
    }
</table>

@code {
    private List<NFCeDto> notas = new();
    
    protected override async Task OnInitializedAsync()
    {
        notas = await Http.GetFromJsonAsync<List<NFCeDto>>("api/nfce");
    }
}
```

## 🔍 Consulta e Filtros

### Por Status
```csharp
var autorizadas = await _context.NFes
    .Where(x => x.Status == "AUTORIZADA")
    .ToListAsync();
```

### Por Período
```csharp
var notas = await _context.NFes
    .Where(x => x.DataEmissao >= inicio && x.DataEmissao <= fim)
    .OrderByDescending(x => x.DataEmissao)
    .ToListAsync();
```

### Por Empresa
```csharp
var notas = await _context.NFes
    .Where(x => x.EmpresaId == empresaId)
    .Include(x => x.Itens)
    .ToListAsync();
```

## 📊 Dashboard em Tempo Real

### Contadores
```csharp
var dashboard = new
{
    Autorizadas = await _context.NFes.CountAsync(x => x.Status == "AUTORIZADA"),
    Pendentes = await _context.NFes.CountAsync(x => x.Status == "PENDENTE"),
    Rejeitadas = await _context.NFes.CountAsync(x => x.Status == "REJEITADA"),
    Canceladas = await _context.NFes.CountAsync(x => x.Status == "CANCELADA"),
    ValorTotal = await _context.NFes
        .Where(x => x.Status == "AUTORIZADA")
        .SumAsync(x => x.ValorTotal)
};
```

## 🔄 Sincronização Offline (PDV)

### 1. Salvar Localmente (SQLite)
```csharp
// PDV offline
var venda = new VendaLocal
{
    Id = Guid.NewGuid(),
    DataVenda = DateTime.Now,
    Status = "PENDENTE_NFCE"
};

await _localDb.Vendas.AddAsync(venda);
await _localDb.SaveChangesAsync();
```

### 2. Fila de Sincronização
```csharp
public class FilaSincronizacao
{
    public async Task ProcessarFilaAsync()
    {
        var vendasPendentes = await _localDb.Vendas
            .Where(x => x.Status == "PENDENTE_NFCE")
            .ToListAsync();
        
        foreach (var venda in vendasPendentes)
        {
            try
            {
                // Tentar emitir NFC-e
                var resultado = await _api.FinalizarVendaAsync(venda.Id);
                
                if (resultado.Sucesso)
                {
                    venda.Status = "SINCRONIZADA";
                    venda.ChaveNFCe = resultado.ChaveNFCe;
                    await _localDb.SaveChangesAsync();
                }
            }
            catch
            {
                // Tentar novamente depois
            }
        }
    }
}
```

### 3. Background Job
```csharp
// Executar a cada 5 minutos
RecurringJob.AddOrUpdate(
    "sincronizar-nfce",
    () => _filaSincronizacao.ProcessarFilaAsync(),
    "*/5 * * * *"
);
```

## 📈 Relatórios

### Vendas com NFC-e
```sql
SELECT 
    v.Id,
    v.DataVenda,
    v.ValorTotal,
    n.Numero AS NumeroNFCe,
    n.ChaveAcesso,
    n.Status
FROM Vendas v
LEFT JOIN NFes n ON n.VendaId = v.Id
WHERE v.DataVenda >= @DataInicio
ORDER BY v.DataVenda DESC;
```

### Faturamento por Status
```sql
SELECT 
    Status,
    COUNT(*) AS Quantidade,
    SUM(ValorTotal) AS ValorTotal
FROM NFes
WHERE DataEmissao >= @DataInicio
GROUP BY Status;
```

## 🔐 Auditoria

### Log de Operações
```csharp
public class NFCeAuditoria
{
    public Guid Id { get; set; }
    public Guid NFCeId { get; set; }
    public string Operacao { get; set; } // EMISSAO, CANCELAMENTO, CONSULTA
    public string Usuario { get; set; }
    public DateTime DataHora { get; set; }
    public string? Detalhes { get; set; }
}
```

## ⚠️ Tratamento de Erros

### Retry Automático
```csharp
var policy = Policy
    .Handle<HttpRequestException>()
    .WaitAndRetryAsync(3, retryAttempt => 
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
    );

await policy.ExecuteAsync(async () => 
{
    await _nfeService.EmitirNFeAsync(request);
});
```

### Fallback
```csharp
if (!nfceResponse.Sucesso)
{
    // Salvar venda mesmo sem NFC-e
    venda.Status = "FINALIZADA_SEM_NFCE";
    await _vendaRepository.UpdateAsync(venda);
    
    // Adicionar à fila de reprocessamento
    await _filaReprocessamento.AdicionarAsync(venda.Id);
}
```

## 🚀 Performance

### Índices Recomendados
```sql
CREATE INDEX idx_nfes_status ON NFes(Status);
CREATE INDEX idx_nfes_dataemissao ON NFes(DataEmissao);
CREATE INDEX idx_nfes_vendaid ON NFes(VendaId);
CREATE INDEX idx_nfes_chaveacesso ON NFes(ChaveAcesso);
CREATE INDEX idx_pix_txid ON Pix(TxId);
CREATE INDEX idx_pix_status ON Pix(Status);
```

### Cache
```csharp
// Cache de 5 minutos
var notas = await _cache.GetOrCreateAsync("nfces-hoje", async entry =>
{
    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
    return await _nfeRepository.GetByDataAsync(DateTime.Today);
});
```

## 📞 Webhooks

### Notificação de Status
```csharp
[HttpPost("webhooks/nfce")]
public async Task<IActionResult> WebhookNFCe([FromBody] NFCeWebhook webhook)
{
    var nfce = await _nfeRepository.GetByChaveAcessoAsync(webhook.ChaveAcesso);
    
    if (nfce != null && webhook.Status == "AUTORIZADA")
    {
        nfce.Autorizar(webhook.ChaveAcesso, webhook.Protocolo, webhook.Xml, webhook.XmlRetorno);
        await _nfeRepository.UpdateAsync(nfce);
    }
    
    return Ok();
}
```

## ✅ Checklist de Implementação

- [x] Criar tabelas NFes e Pix
- [x] Criar repositórios
- [x] Implementar handler de finalização
- [x] Salvar NFC-e no banco
- [x] Vincular NFC-e à venda
- [x] Criar endpoints de consulta
- [x] Criar página frontend
- [x] Implementar dashboard
- [ ] Implementar sincronização offline
- [ ] Implementar fila de reprocessamento
- [ ] Implementar webhooks
- [ ] Implementar auditoria
- [ ] Criar relatórios
