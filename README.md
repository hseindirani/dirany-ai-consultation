# Dirany AI Consultation

Dirany AI Consultation is a full-stack, AI-assisted barber consultation platform built around a real barbershop use case.

The application helps barbers and customers explore hairstyle and beard options, generate realistic AI previews, agree on a look, and preserve the consultation, barber notes, and final haircut for future visits.

## Features

- Customer search and consultation history
- Front and side reference photos
- Hair and beard style shortlists
- AI-generated hair, beard, and combined previews
- Persistent generated previews
- Barber notes and final haircut photos
- Completed consultation history
- Private cloud image storage

## Tech Stack

**Frontend**
- Angular
- TypeScript

**Backend**
- C# / ASP.NET Core
- Entity Framework Core
- PostgreSQL
- REST APIs
- xUnit

**Cloud & AI**
- Azure Container Apps
- Azure Container Registry
- Azure Database for PostgreSQL
- Azure Blob Storage
- Azure AI / GPT Image
- Managed Identity & RBAC

**DevOps**
- Docker & Docker Compose
- GitHub Actions CI/CD
- Git / GitHub

## Architecture

```text
Angular
   │
   │ REST API
   ▼
ASP.NET Core
   │
   ├──► Azure Database for PostgreSQL
   │
   ├──► Azure Blob Storage
   │       Managed Identity / RBAC
   │
   └──► Azure AI
           GPT Image
```

The frontend and backend are containerized and deployed independently on Azure Container Apps, with Docker images stored in Azure Container Registry.

PostgreSQL stores customers, consultations, style selections, notes, and image metadata. Customer and generated images are stored privately in Azure Blob Storage, while Azure AI generates hairstyle and beard previews.

The application persists generated previews instead of regenerating them during normal reads, reducing unnecessary AI calls and preserving consultation history.

## Consultation Flow

```text
Customer
   ↓
Start Consultation
   ↓
Upload Front / Side Photos
   ↓
Choose Hair & Beard Styles
   ↓
Generate AI Previews
   ↓
Select Preferred Look
   ↓
Perform Haircut
   ↓
Save Notes + Final Result
   ↓
Consultation History
```

## Cloud & Security

The application uses Azure-managed infrastructure for its deployed environment.

- Private Blob Storage for customer images
- Managed Identity and RBAC for Blob access
- Secrets kept outside source control
- HTTPS through Azure Container Apps
- Separate GitHub Actions CI/CD pipelines for automated frontend and backend deployments

## Local Development

The application can also run locally using Docker Compose:

```bash
docker compose up --build
```

Local development uses PostgreSQL and external configuration for credentials and secrets.

## Project Goal

This project combines a real business use case with full-stack software engineering, AI integration, cloud infrastructure, and DevOps.

Rather than building a standalone AI image generator, the AI functionality is integrated into a persistent consultation workflow that can be used before, during, and after a haircut.