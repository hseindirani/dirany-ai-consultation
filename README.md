# Dirany AI Consultation

Dirany AI Consultation is a full-stack, AI-assisted barber consultation platform built around a real use case from a barbershop.

The application helps a barber and customer explore hairstyle and beard options before a haircut, generate realistic AI-assisted previews, agree on a look, and save the final result and consultation history for future visits.

The goal of the project is not simply to generate hairstyle images. It is to create a complete consultation workflow that combines AI-assisted visualization with customer history, barber knowledge, and persistent consultation data.

## Current Status

**V1 / MVP is working end-to-end locally.**

The application currently supports:

- Creating new customers
- Searching and selecting existing customers
- Viewing previous consultations
- Starting and continuing consultations
- Front and side customer reference photos
- Hair and beard style catalogs
- Hair and beard candidate shortlists
- AI-generated hairstyle previews
- Front and side hairstyle previews
- AI-generated beard previews
- Combined hair + beard previews
- Persisted generated previews
- Barber consultation notes
- Uploading the real finished haircut
- Completing consultations
- Reopening completed consultations and viewing their saved history

The backend and frontend are containerized with Docker and can be run locally with Docker Compose.

Cloud deployment and production infrastructure are the next phase of the project.

## Consultation Flow

```text
Find existing customer / Create new customer
                    ↓
            Start consultation
                    ↓
        Upload reference photos
             Front + Side
                    ↓
        Choose hairstyle options
                    ↓
          Choose beard options
                    ↓
        Generate AI previews
                    ↓
       Select preferred result
                    ↓
     Generate combined preview
                    ↓
          Perform haircut
                    ↓
          Add barber notes
                    ↓
     Upload real final result
                    ↓
       Complete consultation
                    ↓
       Saved customer history
```

## Why I Built It

The idea came from a real problem in barber consultations.

Customers often have an idea of what they want but have difficulty visualizing how a hairstyle or beard style will look on them. Reference photos help, but they show the style on somebody else.

Dirany AI Consultation adds an AI-assisted visualization step while keeping the barber involved in the decision.

Instead of being a standalone image generator, the AI is part of a larger workflow:

- the barber creates a shortlist of realistic options
- the customer can compare previews
- the chosen consultation is preserved
- the actual finished haircut is saved
- barber notes and previous results can be retrieved on future visits

This turns a one-time AI image into useful consultation history.

## Tech Stack

### Backend

- C#
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- REST APIs
- xUnit

### Frontend

- Angular
- TypeScript
- Reactive Forms
- Angular HttpClient

### AI

- Microsoft Foundry / Azure AI
- GPT Image model
- Image editing/generation APIs

### Infrastructure

- Docker
- Docker Compose
- Git / GitHub

### Planned Cloud Infrastructure

- Azure Container Apps
- Azure Blob Storage
- Azure Database for PostgreSQL
- Azure Key Vault
- Application Insights
- GitHub Actions
- Infrastructure as Code with Bicep or Terraform

## Architecture

The application currently follows a straightforward full-stack architecture:

```text
┌─────────────────────┐
│      Angular        │
│      Frontend       │
└──────────┬──────────┘
           │ HTTP / REST
           ▼
┌─────────────────────┐
│    ASP.NET Core     │
│        API          │
│                     │
│ Business Rules      │
│ Consultation Flow   │
│ AI Integration      │
└──────┬────────┬─────┘
       │        │
       │        └──────────────► Azure AI
       │                         Image Generation
       │
       ▼
┌─────────────────────┐
│     PostgreSQL      │
│                     │
│ Customers           │
│ Consultations       │
│ Candidates          │
│ Image Metadata      │
└─────────────────────┘
```

Images currently use local storage during development.

The storage layer is abstracted through `IImageStorage`, allowing local storage to be replaced by Azure Blob Storage without changing the consultation domain logic.



## AI Integration

AI generation is deliberately separated from the core consultation domain.

The application sends the customer's original image together with controlled style instructions to the configured Azure image model.

The generated image is then persisted as part of the consultation instead of being regenerated whenever the page is opened.

This allows the application to:

- avoid unnecessary AI calls
- preserve previous results
- associate previews with specific hairstyle and beard candidates
- restore previews after refresh
- keep AI generation separate from normal application reads

Hair previews support both front and side reference images.

Combined previews are associated with the exact selected hair and beard candidate pair.

## Customer History

Existing customers can be searched by name or phone number.

Their consultation history includes both ongoing and completed consultations, with the newest consultation shown first.

An ongoing consultation can be continued, while a completed consultation can be reopened in read-only form to view:

- previous selections
- generated previews
- barber notes
- final haircut result

This creates a persistent haircut history that can be used as a reference during future visits.

## Local Development

### Requirements

- .NET SDK
- Node.js / npm
- Angular CLI
- PostgreSQL
- Docker Desktop


### Docker

The frontend and backend are containerized and can be started using:

```bash
docker compose up --build
```

The current local Docker setup connects the backend container to PostgreSQL running on the host machine.

Environment variables and secrets are not committed to the repository.

## Security & Privacy

Customer photos are treated as application data and are not committed to the repository.

Secrets such as database credentials and Azure API keys are kept outside source control.

Production storage will use private Azure Blob Storage rather than the container filesystem.

Additional production security and data-retention controls will be introduced during the cloud deployment phase.

## Next Steps

The next phase focuses on moving the working V1 from local development to a production-oriented Azure architecture:

1. Azure Blob Storage for persistent image storage
2. Azure-hosted PostgreSQL
3. ASP.NET Core deployment with Azure Container Apps
4. Frontend cloud deployment
5. Production configuration and secret management
6. GitHub Actions CI/CD
7. Application Insights and observability
8. Infrastructure as Code

## Project Goal

Dirany AI Consultation is both a real business experiment and a software engineering portfolio project.

The project is designed to demonstrate how a practical problem can be taken from idea to a working full-stack system using backend engineering, databases, frontend development, AI integration, containerization, and eventually cloud infrastructure and DevOps.