# SuperERP API Documentation

## Endpoints Disponíveis

### Clientes

#### POST /api/v1/clientes
Cria um novo cliente

**Request Body:**
```json
{
  "nome": "João Silva",
  "cpfCnpj": "12345678900",
  "email": "joao@email.com",
  "telefone": "(11) 98765-4321"
}
```

**Response:** 201 Created
```json
{
  "id": "guid",
  "nome": "João Silva",
  "cpfCnpj": "12345678900",
  "email": "joao@email.com",
  "telefone": "(11) 98765-4321",
  "criadoEm": "2025-01-01T00:00:00Z",
  "ativo": true
}
```

### Produtos

#### POST /api/v1/produtos
Cria um novo produto

**Request Body:**
```json
{
  "sku": "PROD001",
  "nome": "Produto Exemplo",
  "descricao": "Descrição do produto",
  "codigoBarras": "7891234567890",
  "precoVenda": 100.00,
  "precoCusto": 50.00
}
```

**Response:** 201 Created
```json
{
  "id": "guid",
  "sku": "PROD001",
  "nome": "Produto Exemplo",
  "descricao": "Descrição do produto",
  "codigoBarras": "7891234567890",
  "precoVenda": 100.00,
  "precoCusto": 50.00,
  "estoqueAtual": 0,
  "criadoEm": "2025-01-01T00:00:00Z",
  "ativo": true
}
```

### Vendas

#### POST /api/v1/vendas
Cria uma nova venda

**Request Body:**
```json
{
  "clienteId": "guid",
  "dataVenda": "2025-01-01T00:00:00Z"
}
```

**Response:** 201 Created
```json
{
  "id": "guid",
  "clienteId": "guid",
  "dataVenda": "2025-01-01T00:00:00Z",
  "valorTotal": 0,
  "status": "ABERTA"
}
```

## Headers

### Multi-tenancy
Todas as requisições devem incluir o header:
```
X-Tenant-Id: {guid-do-tenant}
```

### Autenticação (Futuro)
```
Authorization: Bearer {token}
```

## Códigos de Status

- **200 OK**: Requisição bem-sucedida
- **201 Created**: Recurso criado com sucesso
- **400 Bad Request**: Dados inválidos
- **401 Unauthorized**: Não autenticado
- **403 Forbidden**: Sem permissão
- **404 Not Found**: Recurso não encontrado
- **500 Internal Server Error**: Erro no servidor
