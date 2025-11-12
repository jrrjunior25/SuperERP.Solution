# Super ERP + Super PDV

Sistema completo de gestão empresarial (ERP) e automação comercial (PDV) desenvolvido em .NET 9.

## 🚀 Tecnologias

- **.NET 9** - Framework principal
- **Clean Architecture + DDD** - Arquitetura de software
- **Entity Framework Core 9** - ORM
- **PostgreSQL** - Banco de dados principal
- **SQLite** - Banco local do PDV
- **Blazor Server** - Interface web do ERP
- **Blazor Hybrid (MAUI)** - PDV desktop/mobile
- **RabbitMQ** - Mensageria
- **MediatR** - CQRS
- **FluentValidation** - Validações
- **xUnit** - Testes unitários

## 📦 Estrutura do Projeto

\\\
SuperERP.Solution/
├── src/
│   ├── Core/
│   │   ├── SuperERP.Domain          # Entidades e regras de negócio
│   │   └── SuperERP.Application     # Casos de uso e DTOs
│   ├── Infrastructure/
│   │   └── SuperERP.Infrastructure  # Repositórios e integrações
│   └── Presentation/
│       ├── SuperERP.API             # Web API RESTful
│       ├── SuperERP.Web             # Interface web (Blazor)
│       └── SuperERP.PDV             # PDV (Blazor Hybrid)
├── tests/
│   ├── SuperERP.Domain.Tests
│   ├── SuperERP.Application.Tests
│   └── SuperERP.API.Tests
├── deploy/
│   ├── docker-compose.yml
│   └── Dockerfile.api
└── build-scripts/
    ├── build.ps1
    └── run.ps1
\\\

## 🛠️ Como Executar

### Pré-requisitos

- .NET 9 SDK
- Docker Desktop (opcional)
- PostgreSQL (ou use o Docker)

### Opção 1: Execução Rápida

\\\powershell
# Iniciar tudo (API + Web)
.\run-all.ps1

# Ou modo desenvolvimento (hot reload)
.\run-dev.ps1

# Parar tudo
.\stop-all.ps1
\\\

### Opção 2: Docker Compose

\\\powershell
cd deploy
docker compose up --build
\\\

Acesse:
- API: http://localhost:5000/swagger
- RabbitMQ Management: http://localhost:15672 (user: supererp, pass: Super@ERP2025!)

### Migrations

\\\powershell
# Criar migration
dotnet ef migrations add InitialCreate --project src\Infrastructure\SuperERP.Infrastructure --startup-project src\Presentation\SuperERP.API

# Aplicar ao banco
dotnet ef database update --project src\Infrastructure\SuperERP.Infrastructure --startup-project src\Presentation\SuperERP.API
\\\

## 📚 Módulos

### ERP (Retaguarda)
- ✅ Cadastros (Clientes, Produtos, Fornecedores)
- ✅ Vendas e Comissões
- ✅ Ordem de Serviço (OS)
- ✅ Financeiro (Contas a Pagar/Receber)
- ✅ Estoque e Compras
- ✅ Emissão de NF-e / NFS-e / NFC-e
- ✅ Relatórios Gerenciais

### PDV (Frente de Caixa)
- ✅ Operação Online/Offline
- ✅ Sincronização Automática
- ✅ Emissão NFC-e (SAT/SP)
- ✅ TEF Dedicado e Smart TEF
- ✅ Controle de Caixa
- ✅ Modo Touchscreen

## 🔐 Multitenancy

O sistema suporta múltiplas empresas (multi-tenant) com isolamento de dados por tenant.

## 📄 Licença

Proprietário - Todos os direitos reservados

## 👨‍💻 Desenvolvido por

Seu Nome / Sua Empresa
