# SuperERP - Frontend Blazor Server

## ✅ Frontend Criado com Sucesso!

### Tecnologias Utilizadas
- **Blazor Server** (.NET 9)
- **MudBlazor** - Componentes UI modernos
- **HttpClient** - Comunicação com API

### Páginas Implementadas

#### 1. Dashboard (/)
- Cards com métricas principais
- Vendas do dia
- Total de clientes
- Total de produtos
- Contas a receber

#### 2. Clientes (/clientes)
- Listagem de clientes
- Cadastro de novos clientes
- Integração com API
- Validação de formulários

#### 3. Produtos (/produtos)
- Listagem de produtos
- Cadastro de novos produtos
- Campos: SKU, Nome, Descrição, Código de Barras, Preços
- Integração com API

#### 4. Vendas (/vendas)
- Listagem de vendas
- Status coloridos (Aberta, Finalizada, Cancelada)
- Visualização de detalhes

### Layout e Navegação

**Menu Lateral com Grupos:**
- 📊 Dashboard
- 🛒 Comercial
  - Clientes
  - Produtos
  - Vendas
- 💰 Financeiro
  - Contas a Receber
  - Contas a Pagar
- 📦 Estoque
  - Movimentos
- 🧾 Fiscal
  - Notas Fiscais

### Como Executar

1. **Certifique-se que a API está rodando:**
   ```bash
   cd src/Presentation/SuperERP.API
   dotnet run
   ```

2. **Execute o Frontend:**
   ```bash
   cd src/Presentation/SuperERP.Web
   dotnet run
   ```

3. **Acesse:**
   - Frontend: http://localhost:5001
   - API: http://localhost:5000/swagger

### Configuração

O arquivo `appsettings.json` contém a URL da API:
```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000"
  }
}
```

### Funcionalidades Implementadas

✅ Layout responsivo com MudBlazor  
✅ Menu de navegação lateral  
✅ Dashboard com cards de métricas  
✅ CRUD de Clientes (Create + List)  
✅ CRUD de Produtos (Create + List)  
✅ Listagem de Vendas  
✅ Integração com API via HttpClient  
✅ Notificações (Snackbar)  
✅ Dialogs para formulários  
✅ Tabelas responsivas  

### Próximas Melhorias

- [ ] Implementar edição e exclusão
- [ ] Adicionar paginação nas tabelas
- [ ] Implementar busca e filtros
- [ ] Adicionar gráficos no dashboard
- [ ] Implementar autenticação
- [ ] Adicionar validações mais robustas
- [ ] Implementar módulo de vendas completo
- [ ] Adicionar relatórios

### Estrutura de Arquivos

```
SuperERP.Web/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor (Layout principal com MudBlazor)
│   │   └── NavMenu.razor (Menu de navegação)
│   ├── Pages/
│   │   ├── Home.razor (Dashboard)
│   │   ├── Clientes.razor (CRUD Clientes)
│   │   ├── Produtos.razor (CRUD Produtos)
│   │   └── Vendas.razor (Listagem Vendas)
│   ├── _Imports.razor (Imports globais)
│   └── App.razor (Root component)
├── wwwroot/
│   └── app.css
├── appsettings.json
└── Program.cs
```

### Componentes MudBlazor Utilizados

- MudAppBar - Barra superior
- MudDrawer - Menu lateral
- MudCard - Cards de conteúdo
- MudTable - Tabelas de dados
- MudDialog - Modais
- MudTextField - Campos de texto
- MudButton - Botões
- MudIconButton - Botões com ícones
- MudSnackbar - Notificações
- MudChip - Tags de status

## 🎨 Design

O frontend utiliza o tema padrão do MudBlazor com:
- Cores primárias azuis
- Layout responsivo
- Ícones Material Design
- Animações suaves
- Feedback visual para ações do usuário
