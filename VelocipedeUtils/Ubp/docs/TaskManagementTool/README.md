# TaskManagementTool

[English](README.md) | [Русский](README.ru.md)

The project allows you to organize tasks by priority, track deadlines and progress for each task.

Functional requirements:

- **Creating and editing tasks**: title, description, priority, deadlines, assigned executor.
- **Grouping tasks by projects**: adding, editing, deleting projects.
- **Assigning priorities**.
- **Ability to group tasks by projects**.
- **Progress tracking**: task statuses (In progress, Completed, Postponed).
- **Notification system**: about upcoming deadlines, changes in tasks.
- **Search and filtering**: search by keywords, filter by date, status, priority, artist.

User Groups:
- students
- freelancers
- project managers
- task performers.

Needs:
- user-friendly interface
- possibility of collaboration
- accessibility from different devices
- data synchronization.

## Architecture

Components and modules:

- Task module: Creating, editing, deleting tasks.
- Projects module: Creating, editing, deleting projects.
- User module: Registration, authorization, profile management.
- Comments module: Adding, editing, deleting comments to tasks.
- Notification module: Notifications about deadlines, changes in tasks.
- Report module: Generating reports on task completion and project progress.

Technologies and tools:

- Backend: ASP.NET 8 (MVC or Blazor)
- Database: PostgreSQL
- ORM: EF Core
- Authentication and Authorization: JWT (JSON Web Token) tokens with user data stored in PostgreSQL.
- Caching: Redis (optional).

Security Mechanisms:

- **Authentication**: JWT tokens for user authentication.
- **Authorization**: Role-based access rights (eg administrator, user, guest).
- **Encryption**: Encrypt sensitive information (such as passwords).
- **Protection against SQL injections**: Use of parameterized queries.
- **Cross-site scripting (XSS) protection**: Validation and escaping of user input.

Main databases and data warehouses:

- **PostgreSQL**: Main database for storing tasks, projects, users, comments.
- **Redis**: Additional storage for caching frequently used data (eg task list, user profiles).

Design Patterns:

- MVC: Model-View-Controller to separate logic, presentation and data.
- Repository: To abstract database access.
- Dependency Injection.
- Factory: To create objects using various parameters.
