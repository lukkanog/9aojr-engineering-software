# Microsserviço de Processamento de Produtos

Este projeto implementa um microsserviço para processamento de produtos utilizando **.NET 8**, **SQLite** e **Docker**, seguindo os princípios **GRASP** (General Responsibility Assignment Software Patterns).

## 🏗️ Arquitetura

O projeto está organizado em camadas seguindo os princípios GRASP:

```
ProductProcessing/
├── Models/          # Entidades de domínio
├── Data/            # Acesso a dados (Repository Pattern)
├── Services/        # Lógica de negócio
└── Controllers/     # Endpoints da API
```

## 📚 Princípios GRASP Implementados

### 1. **Information Expert (Especialista da Informação)**
- A classe `Product` implementa métodos como `IsValid()`, `CalculateDiscountedPrice()` e `IsAvailable()`.

### 2. **Creator (Criador)**
- `ProductDbContext` é responsável por criar e gerenciar entidades `Product` no banco de dados.

### 3. **Controller (Controlador)**
- `ProductsController` recebe requisições HTTP e coordena as operações, delegando para `ProductService`.

### 4. **Low Coupling (Baixo Acoplamento)**
- Uso de interfaces (`IProductRepository`, `IProductService`) e injeção de dependências.

### 5. **High Cohesion (Alta Coesão)**
- `ProductService` contém toda a lógica de negócio relacionada a produtos.
- `ProductRepository` contém apenas operações de acesso a dados.

## 🎨 Padrões de Design Implementados

### 1. **Repository Pattern**
- `IProductRepository` e `ProductRepository` encapsulam todas as operações de banco de dados.

### 2. **Dependency Injection (Injeção de Dependências)**
- Configurado em `Program.cs` usando o container de DI do ASP.NET Core.

### 3. **Layered Architecture (Arquitetura em Camadas)**
- **Apresentação**: `ProductsController` - Endpoints da API
- **Negócio**: `ProductService` - Lógica de negócio
- **Acesso a Dados**: `ProductRepository` - Operações de banco
- **Domínio**: `Product` - Modelo de dados

## 🚀 Como Executar

### Usando Docker Compose (Recomendado)

```bash
# Construir e executar
docker-compose up --build

# A API estará disponível em:
# - http://localhost:5000
# - Swagger UI: http://localhost:5000/swagger
```

### Executando Localmente

```bash
cd ProductProcessing
dotnet restore
dotnet run

# A API estará disponível em:
# - http://localhost:5000
# - Swagger UI: http://localhost:5000/swagger
```

## 📡 Endpoints da API

- `GET /api/products` - Listar todos os produtos
- `GET /api/products/{id}` - Buscar produto por ID
- `POST /api/products` - Criar novo produto
- `PUT /api/products/{id}` - Atualizar produto
- `DELETE /api/products/{id}` - Deletar produto
- `GET /api/products/{id}/discount?percentage={value}` - Calcular preço com desconto

### Exemplo de Requisição

```json
POST /api/products
{
  "name": "Notebook",
  "description": "Notebook Dell Inspiron",
  "price": 3500.00,
  "stock": 10
}
```

## 🗄️ Banco de Dados

O projeto usa **SQLite** para armazenamento de dados:
- Arquivo: `products.db`
- ORM: Entity Framework Core
- Migrations: Aplicadas automaticamente na inicialização

## 🛠️ Tecnologias

- **.NET 8.0** - Framework
- **ASP.NET Core Web API** - API REST
- **Entity Framework Core** - ORM
- **SQLite** - Banco de dados
- **Docker** - Containerização
- **Swagger/OpenAPI** - Documentação da API

## 📝 Estrutura do Projeto

```
9aojr-engineering-software/
├── ProductProcessing/
│   ├── Controllers/         # Controladores da API
│   ├── Models/             # Modelos de domínio
│   ├── Data/               # Repositórios e DbContext
│   ├── Services/           # Serviços de negócio
│   ├── Program.cs          # Configuração da aplicação
│   └── appsettings.json    # Configurações
├── Dockerfile              # Imagem Docker
├── docker-compose.yml      # Orquestração de containers
└── README.md              # Este arquivo
```

## 🎯 Benefícios da Arquitetura

1. **Testabilidade**: Interfaces permitem fácil criação de mocks
2. **Manutenibilidade**: Responsabilidades claras e separadas
3. **Extensibilidade**: Fácil adicionar novas funcionalidades
4. **Baixo Acoplamento**: Mudanças em uma camada não afetam outras
5. **Alta Coesão**: Código relacionado agrupado logicamente