# 🚀 Super ERP - Sistema Enterprise Completo

Sistema profissional de gestão empresarial (ERP) e automação comercial (PDV) com integrações reais de mercado.

## ✨ Diferenciais

### 💰 Integrações Reais
- ✅ **PIX Gerencianet/Efí** - Geração de QR Code dinâmico com webhook
- ✅ **NF-e/NFC-e ACBr** - Emissão fiscal homologada SEFAZ
- ✅ **Boleto Bancário** - Principais bancos brasileiros
- ✅ **TEF Integrado** - SiTef, PayGo, Rede, Cielo

### 🏗️ Arquitetura Enterprise
- Clean Architecture + DDD
- CQRS com MediatR
- Background Jobs (Hangfire)
- Cache distribuído (Redis)
- Observabilidade completa
- Health Checks

### 🔐 Segurança
- JWT + Refresh Token
- Criptografia AES-256
- Auditoria completa
- LGPD Compliance
- Rate Limiting

## 📦 Stack Tecnológica

```
Backend:     .NET 9, EF Core 9, Dapper
Database:    PostgreSQL, Redis, SQLite (PDV)
Frontend:    Blazor Server, Blazor Hybrid (MAUI)
Messaging:   RabbitMQ
Jobs:        Hangfire
Logs:        Serilog, Seq
Fiscal:      ACBr.Net
Pagamentos:  Gerencianet SDK, QRCoder
PDF:         QuestPDF
Tests:       xUnit, FluentAssertions
```

## 🚀 Quick Start

### 1. Pré-requisitos

```bash
# Instalar .NET 9 SDK
winget install Microsoft.DotNet.SDK.9

# Instalar Docker Desktop
winget install Docker.DockerDesktop

# Instalar PostgreSQL (ou use Docker)
winget install PostgreSQL.PostgreSQL
```

### 2. Configuração

**appsettings.Development.json**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=supererp_dev;Username=postgres;Password=postgres",
    "Redis": "localhost:6379"
  },
  "Gerencianet": {
    "ClientId": "SEU_CLIENT_ID",
    "ClientSecret": "SEU_CLIENT_SECRET",
    "ChavePix": "sua-chave@pix.com",
    "Homologacao": true
  },
  "NFe": {
    "Homologacao": true,
    "CertificadoPath": "C:\\certificados\\certificado.pfx",
    "SenhaCertificado": "senha123"
  }
}
```

### 3. Executar

```powershell
# Subir infraestrutura (PostgreSQL, Redis, RabbitMQ)
cd deploy
docker-compose up -d

# Aplicar migrations
cd ..\src\Presentation\SuperERP.API
dotnet ef database update --project ..\..\Infrastructure\SuperERP.Infrastructure

# Executar API
dotnet run

# Executar Web (outro terminal)
cd ..\SuperERP.Web
dotnet run

# Executar PDV (outro terminal)
cd ..\SuperERP.PDV
dotnet run
```

Acesse:
- **API**: http://localhost:5000/swagger
- **Web**: http://localhost:5001
- **Hangfire**: http://localhost:5000/hangfire

## 📚 Documentação

- [Guia de Integração](docs/GUIA_INTEGRACAO.md) - PIX, NF-e, Certificado Digital
- [Melhorias Profissionais](docs/MELHORIAS_PROFISSIONAIS.md) - Arquitetura e recursos
- [API Documentation](docs/API.md) - Endpoints e exemplos

## 🔌 Integrações

### PIX (Gerencianet)

```http
POST /api/pix/gerar
{
  "empresaId": "guid",
  "chavePix": "sua-chave@pix.com",
  "valor": 100.50,
  "expiracaoMinutos": 30
}
```

**Resposta**: QR Code, PIX Copia e Cola, imagem Base64

### NF-e (ACBr)

```http
POST /api/nfe/emitir
{
  "emitenteCnpj": "12345678000190",
  "destinatarioCpfCnpj": "12345678901",
  "numero": "1",
  "serie": "1",
  "modelo": "55",
  "itens": [...]
}
```

**Resposta**: Chave de acesso, protocolo, XML

## 🏢 Módulos

### ERP (Retaguarda)
- Cadastros (Clientes, Produtos, Fornecedores)
- Vendas e Comissões
- Ordem de Serviço (OS)
- Financeiro (Contas a Pagar/Receber)
- Estoque e Compras
- Emissão NF-e/NFS-e/NFC-e
- Relatórios Gerenciais
- Dashboard em tempo real

### PDV (Frente de Caixa)
- Operação Online/Offline
- Sincronização automática
- Emissão NFC-e
- TEF integrado
- Controle de caixa
- Impressora térmica
- Leitor código de barras
- Modo touchscreen

## 🧪 Testes

```powershell
# Executar todos os testes
dotnet test

# Com cobertura
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Testes específicos
dotnet test --filter "Category=Integration"
```

## 📊 Monitoramento

### Health Checks
```http
GET /health
```

### Logs
```bash
# Tempo real
tail -f logs/supererp-*.log

# Seq (UI)
http://localhost:5341
```

### Métricas
```http
GET /metrics
```

## 🐳 Docker

```bash
# Build
docker build -t supererp-api -f deploy/Dockerfile.api .

# Run
docker-compose up -d
```

## 🔒 Segurança

### Certificado Digital
- Suporte A1 (.pfx) e A3 (token/smartcard)
- Armazenamento seguro
- Renovação automática

### Dados Sensíveis
- CPF/CNPJ criptografados
- Certificados em vault
- Logs sem dados sensíveis

### Auditoria
- Todas as operações registradas
- Rastreabilidade completa
- Conformidade LGPD

## 📈 Performance

- Cache Redis para consultas frequentes
- Queries otimizadas com Dapper
- Índices no banco de dados
- Compressão Gzip/Brotli
- CDN para assets estáticos

## 🌐 Multitenancy

Isolamento completo por tenant:
- Dados segregados
- Configurações independentes
- Certificados por empresa

## 💼 Licenciamento

### Planos

**Básico** - R$ 99/mês
- 1 usuário
- 1 PDV
- Suporte email

**Profissional** - R$ 299/mês
- 5 usuários
- 3 PDVs
- NF-e ilimitada
- Suporte prioritário

**Enterprise** - Sob consulta
- Usuários ilimitados
- PDVs ilimitados
- White label
- Suporte 24/7

## 🤝 Contribuindo

```bash
# Fork o projeto
git clone https://github.com/seu-usuario/SuperERP.Solution

# Crie uma branch
git checkout -b feature/nova-funcionalidade

# Commit suas mudanças
git commit -m "feat: adiciona nova funcionalidade"

# Push para o branch
git push origin feature/nova-funcionalidade

# Abra um Pull Request
```

## 📞 Suporte

- **Email**: suporte@supererp.com.br
- **WhatsApp**: (11) 99999-9999
- **Discord**: https://discord.gg/supererp
- **Documentação**: https://docs.supererp.com.br

## 📄 Licença

Proprietário - Todos os direitos reservados

---

**Desenvolvido com ❤️ usando .NET 9**
