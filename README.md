# Guia de Hotéis - Guia do projeto

## Sobre o Projeto

O **Guia de Hotéis** é uma API desenvolvida para facilitar a consulta de hotéis, reservas e faturamento mensal. A aplicação possui um **backend em .NET**, um **frontend em React com Vite** e um **banco de dados PostgreSQL**. Para testar os endpoints, utilizamos o **Swagger**, e toda a infraestrutura roda dentro de containers Docker.

O frontend está funcional, mas com um design básico, pois o foco inicial foi a implementação da API.

---

## Tecnologias Utilizadas

- **Backend:** .NET 9
- **Frontend:** React + Vite
- **Banco de Dados:** PostgreSQL 17
- **Autenticação:** Token JWT
- **Containerização:** Docker + Docker Compose
- **Documentação da API:** Swagger

---

## 💪 Pré-requisitos

Certifique-se de ter o **Docker** e o **Docker Compose** instalados. Caso não tenha, siga os passos abaixo:

```sh
# Atualizar pacotes
sudo apt update && sudo apt upgrade -y

# Instalar dependências
sudo apt install -y ca-certificates curl gnupg

# Adicionar chave GPG e repositório do Docker
sudo install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo tee /etc/apt/keyrings/docker.asc > /dev/null
sudo chmod a+r /etc/apt/keyrings/docker.asc

echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] \
https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | \
sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

# Instalar Docker e Docker Compose
sudo apt update
sudo apt install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin

# Adicionar o usuário ao grupo Docker
sudo usermod -aG docker $USER
newgrp docker

# Verificar instalação
docker --version
docker compose version
```

---

## 🚀 Como Rodar o Projeto

1️⃣ Clone este repositório:

```sh
git clone https://github.com/RenataGodoy/GuiaDeHoteis.git
cd GuiaDeHoteis
```

2️⃣ Construa e suba os containers:

```sh
docker compose up --build -d
```

3️⃣ Acesse a aplicação no navegador:

```
Frontend: http://localhost:5173
Backend: http://localhost:8000
Swagger: http://localhost:8000/swagger
```

Para parar os containers:

```sh
docker compose down
```

---

## 🔍 Endpoints Disponíveis

### 🌐 Rotas Públicas:

- **`GET /`** → Página inicial do frontend
- **`POST /api/auth/login`** → Realiza login e retorna um token JWT
- **`POST /api/auth/register`** → Registra usuario e retorna um token JWT

### 🔒 Rotas Protegidas (Usuário autenticado):

- **`GET /user-dashboard`** → Página do usuário 
- **`GET /api/reservations`** → Retorna todas as reservas

### 🚫 Rota Oculta (Admin):

- **`GET /api/admin-dashboard`** → Retorna o faturamento mensal (apenas admin)
  - **Requer token JWT de admin**

---

## 💪 Comandos úteis do Docker

🔹 **Ver logs do container:**

```sh
docker compose logs -f
```

🔹 **Reiniciar um serviço específico:**

```sh
docker compose restart nome-do-servico
```

🔹 **Remover containers, volumes e imagens sem uso:**

```sh
docker system prune -a
```

Agora é só rodar e testar! 🚀

## Considerações Finais
O projeto ainda precisa de melhorias no frontend, pois não houve tempo suficiente para refinar a interface. No entanto, a API está totalmente funcional e pode ser testada via Swagger.

Qualquer sugestão ou contribuição é bem-vinda! 🚀
