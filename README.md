## Sho8lana – Freelancing Platform (ASP.NET Core Web API)

Sho8lana is a freelancing platform that connects **clients** and **freelancers** while hiding as much implementation detail and operational complexity from the client as possible.  
It focuses on structured project delivery, collaborative work between freelancers, and a clean, layered backend architecture.

## Demo

- **Video demo**: [Sho8lana on LinkedIn](https://www.linkedin.com/posts/abohend_proud-to-introduce-final-project-sho8lana-activity-7231306096479551491-886C?utm_source=share&utm_medium=member_desktop&rcm=ACoAADYILr4BrAqhr_cK7b9tBZ_z-avbUcCz9Qk)

## Core Features

- **Role-based platform**
  - Separate experiences for **Client**, **Freelancer**, and **Admin**.
- **Project lifecycle**
  - Clients can create and manage projects, receive proposals, and accept/reject them.
  - Once a project is accepted, it can be delivered and marked as completed.
- **Job subdivision**
  - Large projects can be split into **jobs** so multiple freelancers can collaborate on the same project.
- **Job proposals between freelancers**
  - Freelancers can propose to work on other freelancers’ jobs, enabling team-based delivery.
- **CV parsing and filtering**
  - Support for parsing CVs and filtering candidates (based on commits like `CV parser and filtering options`).
- **Real-time communication**
  - Integrated chat built with **SignalR** for real-time messaging between users.
- **Pseudo payment workflow**
  - A controlled “pseudo payment” process when accepting project proposals to simulate or integrate with payments.
- **Robust backend architecture**
  - N-Tier / layered architecture (based on commits like `Applying NTier Architecture`).
  - Global exception handling via middleware.

## Roles & Capabilities

The platform includes three main roles:

1. **Client**
2. **Freelancer**
3. **Admin**

### Client

**What can a Client do?**

- Register and authenticate:
  - `POST api/Account/register/client`
  - `POST api/Account/login`
- Manage profile:
  - `GET/PUT/DELETE api/Client/{id}`
- Manage projects:
  - `GET/POST/PUT/DELETE api/Project`
- Handle project proposals:
  - View proposals for own projects: `GET api/ProjectProposal/{projectId}`
  - Reply to a project proposal (accept/reject / proceed with pseudo payment): `POST api/Project/{projectProposalId}`

### Freelancer

**What can a Freelancer do?**

- Register and authenticate:
  - `POST api/Account/register/freelancer`
  - `POST api/Account/login`
- Manage profile:
  - `GET/PUT/DELETE api/Freelancer/{id}`
- Discover and propose on projects:
  - View all posted projects: `GET api/Project`
  - Create a project proposal: `POST api/ProjectProposal/{projectId}`
- Manage jobs for accepted projects:
  - Create jobs for an accepted project: `POST api/Job/{projectId}`
  - (Large projects can be subdivided into multiple jobs so other freelancers can collaborate.)
- Collaborate with other freelancers:
  - Create a job proposal to join another freelancer’s job: `POST api/JobProposal/{jobId}`
  - View all job proposals for a job: `GET api/JobProposal/{jobId}`
  - View incoming job proposals for a freelancer: `GET api/JobProposal/{freelancerId}`
  - Reply to a job proposal (accept / reject): `POST api/Job/{jobProposalId}`

### Admin

**What can an Admin do?**

- Authenticate:
  - `POST api/Account/login`
- Manage platform taxonomy:
  - CRUD categories: `api/Category`
  - CRUD skills: `api/Skill`

## High-Level Architecture

- **API Layer**: ASP.NET Core Web API exposing endpoints for accounts, projects, proposals, jobs, chat, etc.
- **Application / Domain Layers**: Encapsulate business logic following an N-Tier architecture.
- **Infrastructure Layer**: Handles persistence and integrations (e.g., database, SignalR hubs).
- **Cross-cutting concerns**:
  - Global exception handling via middleware.
  - Centralized validation and response patterns.

## Getting Started (Backend)

1. **Prerequisites**
   - .NET SDK installed.
   - A running SQL database (or the provider configured in the project).
2. **Setup**
   - Clone the repository.
   - Configure connection strings and other environment-specific settings in `appsettings.json` / environment variables.
3. **Run**
   - From the API project directory, run:
     
     ```bash
     dotnet run
     ```
   - Browse the exposed endpoints (e.g., via Swagger if enabled) and start interacting with the roles above.

## Notes

- The commit history includes features like global exception middleware, N-Tier architecture, CV parsing, project/job proposals, pseudo payment flow, and real-time chat.  
- This README is focused on giving a high-level overview; for deeper details, explore the codebase and API controllers.
