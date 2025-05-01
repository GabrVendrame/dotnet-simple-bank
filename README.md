# 🏦 dotnet-simple-bank

API RESTful simplificada de um banco digital, desenvolvida com ASP.NET Core, JWT, Identity e SQL Server.
Projeto baseado no desafio de backend do PicPay.

---

## 💻 Tecnologias

- ASP.NET Core 9
- Entity Framework Core
- ASP.NET Identity
- JWT Authentication
- Docker + Docker Compose
- SQL Server
- Scalar (para documentação OpenAPI)

---

## ▶️ Como rodar o projeto

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Docker Compose](https://docs.docker.com/compose/)

### ⚙️ Configuração do JWT Token

Antes de rodar o projeto é necessário definir as variáveis para a geração de tokens JWT no [appsettings.json](./appsettings.json).

```json
{
  "JWT": {
    "Secret": "a-valid-string-secret-that-is-at-least-512-bits-long-which-is-very-long",
    "Issuer": "simplebank",
    "Audience": "simplebank"
  }
}
```

- `Secret`: chave pra assinar o token, utiliza algoritmo HS512. Escolha uma string com pelo menos 64 caracteres.
- `Issuer`: identificação de quem emite o token. (Recomendação: utilizar o exemplo acima)
- `Audience`: identificação de quem pode consumir o token. (Recomendação: utilizar o exemplo acima)

### 🐋 Subindo com Docker Compose

```bash
docker compose up --build
```

A API estará acessível em:
[http://localhost:5118](http://localhost:5118)

E o banco na porta `1433`.

---

## 📖 Documentação da API

A documentação interativa é gerada automaticamente via Scalar (OpenAPI 3.0) e pode ser acessada através da rota `/scalar`
ou pela url [http://localhost:5118/scalar](http://localhost:5118/scalar)

- Obs: o projeto deve estar rodando para o link ser acessível.

Para endpoints protegidos, é necessário adicionar o header `Authentication: Bearer SEU_JWT_TOKEN`

### Scalar

Exemplo de header adicionado a requisição pelo Scalar
![Exemplo Header Scalar](https://i.imgur.com/icptbKP.png)

### Postman

Exemplo de header adicionado a requisição pelo Postman
![Exemplo Header Postman](https://imgur.com/Ef9egjI.png)

---

## 🔗 Endpoints

| Método | Rota                | Descrição                          | Autenticação |
| ------ | ------------------- | ---------------------------------- | ------------ |
| GET    | `/`                 | Verifica se a API está funcionando | ❌           |
| POST   | `/v1/User/register` | Cadastra um novo usuário           | ❌           |
| POST   | `/v1/User/login`    | Autentica usuário e gera token JWT | ❌           |
| GET    | `/v1/User`          | Lista o usuário logado             | ✅           |
| GET    | `/v1/User/{id}`     | Consulta um usuário pelo ID        | ✅           |
| PUT    | `/v1/User`          | Atualiza telefone do usuário       | ✅           |
| DELETE | `/v1/User/{id}`     | Remove usuário pelo ID             | ✅           |
| POST   | `/v1/Transfer`      | Realiza uma transferência          | ✅           |
| GET    | `/v1/Transfer/{id}` | Consulta transferência por ID      | ✅           |
| GET    | `/v1/Balance`       | Consulta o saldo do usuário        | ✅           |
| PUT    | `/v1/Balance`       | Adiciona saldo ao usuário          | ✅           |

---

## 📚 Exemplos de DTOs

### 👤 Criar Usuário (`CreateUserDto`)

```json
{
  "fullName": "Fulano da Silva",
  "email": "fulano@example.com",
  "password": "SenhaForte123!",
  "cpfCnpj": "12345678900",
  "phone": "+5511999999999"
}
```

### 🔑 Login (`LoginDto`)

```json
{
  "email": "fulano@example.com",
  "password": "SenhaForte123!"
}
```

### 💸 Criar Transferência (`CreateTransferDto`)

```json
{
  "amount": 50.0,
  "payeeID": "guid-do-recebedor"
}
```

### 💰 Adicionar Saldo (`AddBalanceDto`)

```json
{
  "balance": 100.0
}
```

### ✏️ Editar telefone (`EditUserDto`)

```json
{
  "phoneNumber": "+5511988888888"
}
```

---

## 📌 Observações

- As transferências utilizam validação externa simulada via API mock (https://util.devi.tools/api/v2/authorize)
- Operações sensíveis como transferência e consulta de saldo são protegidas por autenticação JWT
- Os dados do banco são persistidos em um volume Docker
- Roles utilizadas: `User` e `Seller` com controle por `IdentityRole`
- O campo `CpfCnpj` é único e obrigatório
- O campo `Email` é único e obrigatório

---

## 🔐 Segurança

- JWT Token é exigido nos endpoints protegidos
- Apenas usuários autenticados podem transferir ou visualizar saldo

---
