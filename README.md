# Freelancing-Website

Freelancing Website that let the client be away of implementation details as could as possible.

## Roles

The website include 3 roles:

1. Client
2. Freelancer
3. Admin

### Client

**What Client Can Do?**

1. Register in the website `api/Account/register/client`
2. Sign in `api/Account/login`
3. Get, Update or Delete his account `api/Client/{id}`
4. CRUD a project `api/project`
5. View proposals for his projects `api/ProjectProposal/{projectId}`
6. Replay to a project proposal `api/project/{projectProposalId}`

### Freelancer

**What Freelancer Can Do?**

1. Register in the website `api/Account/register/freelancer`
2. Sign in `api/Account/login`
3. Get, Update or Delete his account `api/Freelancer/{id}`
4. View all posted projects `api/Project`
5. Create a project proposal `api/ProjectProposal/{projectId}`
6. Create a jobs for a accepted project `api/Job/{projectId}`
   > [!NOTE]
   > Large Projects could be subdivided by freelancer to allow other freelancer to work with him on the same project.
7. Create a job proposal for other `freelancer api/JobProposal/{jobId}`
8. View all job proposals for a job `api/JobProposal/{jobId}`
9. View coming job proposals `api/jobProposal/{freelancerId}`
10. Replay to a job proposal `job/{jobProposalId}`

### Admin

**What Admin Can Do?**

1. Sign in `api/Account/login`
2. CRUD a category `api/Category`
3. CRUD a skill `api/Skill`
