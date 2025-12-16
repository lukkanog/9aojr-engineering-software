# Guia de Uso Rápido - Product Processing API

## Pré-requisitos

- .NET 8.0 SDK (para execução local)
- Docker e Docker Compose (para execução containerizada)

## Execução Local

### 1. Restaurar dependências e executar

```bash
cd ProductProcessing
dotnet restore
dotnet run
```

A API estará disponível em: http://localhost:5000

### 2. Acessar documentação Swagger

Abra o navegador em: http://localhost:5000/swagger

## Execução com Docker

### 1. Build e execução

```bash
docker compose up --build
```

A API estará disponível em: http://localhost:5000

### 2. Parar os containers

```bash
docker compose down
```

## Testando a API

### Usar o script de teste incluído

```bash
# Certifique-se de que a API está rodando
chmod +x test-api.sh
./test-api.sh
```

### Testes manuais com curl

#### Listar todos os produtos
```bash
curl http://localhost:5000/api/products
```

#### Criar um novo produto
```bash
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Notebook Dell",
    "description": "Notebook Dell Inspiron 15",
    "price": 3500.00,
    "stock": 10
  }'
```

#### Buscar produto por ID
```bash
curl http://localhost:5000/api/products/1
```

#### Calcular preço com desconto
```bash
curl "http://localhost:5000/api/products/1/discount?percentage=10"
```

#### Atualizar produto
```bash
curl -X PUT http://localhost:5000/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Notebook Dell Atualizado",
    "description": "Notebook Dell Inspiron 15 - 16GB RAM",
    "price": 3800.00,
    "stock": 8
  }'
```

#### Deletar produto
```bash
curl -X DELETE http://localhost:5000/api/products/1
```

## Estrutura dos Dados

### Modelo de Produto

```json
{
  "id": 1,
  "name": "Nome do Produto",
  "description": "Descrição do produto",
  "price": 100.00,
  "stock": 50,
  "createdAt": "2025-12-16T23:00:00Z",
  "updatedAt": null
}
```

### Validações

- `name`: obrigatório, máximo 200 caracteres
- `description`: opcional, máximo 1000 caracteres
- `price`: deve ser >= 0
- `stock`: deve ser >= 0

## Banco de Dados

- **SQLite** é usado como banco de dados
- Arquivo de banco: `products.db` (criado automaticamente)
- Quando usando Docker, o banco fica persistido no volume `product-data`

## Solução de Problemas

### Porta já em uso
Se a porta 5000 já estiver em uso, você pode mudar a porta no `docker-compose.yml` ou ao executar localmente:

```bash
dotnet run --urls "http://localhost:5050"
```

### Resetar banco de dados local
```bash
rm ProductProcessing/products.db*
```

### Ver logs do Docker
```bash
docker compose logs -f
```

## Mais Informações

Consulte o [README.md](README.md) para detalhes sobre os princípios GRASP e padrões de design implementados.
