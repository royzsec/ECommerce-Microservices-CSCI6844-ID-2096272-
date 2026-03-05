# E-Commerce Microservices System

## Project Overview

This project implements a containerized microservices-based E-Commerce system using:

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core (Async)
- Database-per-service architecture
- Docker & Docker Compose
- Synchronous inter-service communication (HttpClient)

The system consists of four independent microservices:

1. Customer Service – Manages customer information  
2. Product Service – Manages product catalog and stock  
3. Order Service – Creates and tracks orders  
4. Payment Service – Processes payments  

Each service runs independently in its own container and maintains its own database.

---

## Architecture

### Microservices Architecture Design

- Each service owns its own SQLite database.
- No shared database between services.
- Order Service communicates synchronously using HttpClient to:
  - Customer Service (validate customer existence)
  - Product Service (validate product existence)
  - Payment Service (process payment)
- All services are containerized using Docker.

### Logical Architecture

- CustomerService  → Customer DB  
- ProductService   → Product DB  
- OrderService     → Order DB  
- PaymentService   → Payment DB  

- OrderService → validates customer & product  
- OrderService → calls PaymentService  

---

## Project Structure

```text
ECommerceMicroservices/
│
├── CustomerService/
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   └── Dockerfile
│
├── ProductService/
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   └── Dockerfile
│
├── OrderService/
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   └── Dockerfile
│
├── PaymentService/
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   └── Dockerfile
│
├── docker-compose.yml
└── ECommerceMicroservices.sln
```
---

Each microservice contains:
- Controllers
- Models
- DbContext
- Dockerfile
- Independent migrations
- Separate database

---

## Technologies Used

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (Async)
- SQLite
- Docker
- Docker Compose
- Swagger (OpenAPI)

---

## How to Run the Project

### Prerequisites

Make sure you have:

- Docker Desktop installed
- .NET 8 SDK (optional if running only via Docker)

---

### Step 1 – Clone the Repository

```bash
git clone https://github.com/royzsec/ECommerce-Microservices-CSCI6844-ID-2096272-.git
cd ECommerceMicroservices
```
###  Step 2 – Build and Run Using Docker
```bash
docker compose up --build
```

### Step 3 – Access Swagger UI
----------------------------------------------------
| Service          | URL                           |
|------------------|-------------------------------|
| Customer Service | http://localhost:6002/swagger |
| Product Service  | http://localhost:6001/swagger |
| Order Service    | http://localhost:6003/swagger |
| Payment Service  | http://localhost:6004/swagger |
----------------------------------------------------
---

## Docker Commands

Stop containers:

```bash
docker compose down
```

Remove volumes:

```bash
docker compose down -v
```

Rebuild system:

```bash
docker compose up --build
```

---
