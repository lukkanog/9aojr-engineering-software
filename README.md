# Microsserviço de Processamento de Produtos
Trabalho realizado para a disciplina de Engineering Software Development da turma 9AOJR do MBA de Engenharia de Software da FIAP

Foi feito um microsserviço utilizando o Github Copilot como base, e criamos 3 test suits: 2 de unidade e 1 de integração

## Microsserviço
O microsserviço é uma API REST para cadastro de produtos, feita com .NET integrando com sqlite

## Como executar

Para executar o microsserviço utilizando Docker Compose:

```bash
docker compose up --build
```

A API ficará disponível na porta configurada no `docker-compose.yml` (por padrão, porta 8080 ou 5000, conforme configuração). A documentação interativa estará disponível em `/swagger`.

Para rodar localmente (sem Docker), é necessário ter o .NET 8 SDK instalado. Em seguida, na pasta `ProductProcessing`:

```bash
dotnet run
```

## Testes

Foram feitos testes utilizando o NUnit e o NSubstitute.

Para executar os testes via CLI do .NET, na raiz do repositório:

```bash
dotnet test
