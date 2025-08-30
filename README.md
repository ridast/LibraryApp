# 📚 LibraryApp
LibraryApp is a modern, containerized full-stack application designed to manage a library's book inventory and lending operations. It leverages a robust **.NET 9 Web API with Razor Pages** for the backend, a responsive **React** frontend for user interaction, and a reliable **SQL Server** database for persistent storage. The entire solution is orchestrated with **Docker Compose** for seamless local development and deployment.

---

## 📝 Description

LibraryApp provides a comprehensive platform for managing books in a library environment. The application allows users to view available books, borrow and return books, and manage the library's inventory through an intuitive web interface. The backend is built with .NET 9 Razor Pages and exposes a RESTful API for all core operations, ensuring scalability and maintainability. Entity Framework Core is used for data access, enabling efficient and secure interactions with the SQL Server database.

The frontend, built with React, offers a clean and user-friendly experience for both library staff and patrons. It communicates with the backend API to display real-time data, handle user actions, and provide feedback. The application is fully containerized, making it easy to set up, run, and scale across different environments using Docker Compose.

**Key features include:**
- Browsing and searching the library's book collection
- Borrowing and returning books with real-time updates
- Adding and deleting book records (for authorized users)
- Validation and error handling for all operations
- Persistent data storage with SQL Server
- Clean separation of concerns between frontend, backend, and database
- Easy local development and deployment with Docker Compose


---

## 🚀 Features
- Full-stack application with modern technologies
- **.NET 9 Web API + Razor Pages** backend
- **React** frontend
- **Entity Framework Core** for database access
- **SQL Server** as the database
- **Docker Compose** for containerized development and deployment

---

## 🛠️ Prerequisites

Before running the project, ensure you have:

- [Docker](https://www.docker.com/products/docker-desktop) installed  
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) *(optional, for local backend development)*  
- [Node.js & npm](https://nodejs.org) *(optional, for frontend development outside Docker)*  

---

## 📂 Project Structure


LibraryApp/
├── docker-compose.yml
├── Dockerfile                 # .NET backend Dockerfile
├── appsettings.json
├── libraryapp-frontend/       # React frontend
│   ├── Dockerfile             # React frontend Dockerfile
│   └── src/                   # React source files
└── src/                       # .NET backend source
    ├── LibraryApp.API/        # API project (Razor Pages)
    ├── LibraryApp.Application/# Application logic
    ├── LibraryApp.Domain/     # Domain entities
    └── LibraryApp.Infrastructure/ # Infrastructure (EF Core, DB)
    ---
 ## Run the application with Docker

git clone repository
cd LibraryApp

docker-compose up --build


After all containers are running properly:
- **You can access the React frontend at:** [http://localhost:3000](http://localhost:3000)
- **The API will be available at:** [http://localhost:8080/books](http://localhost:8080/books)

---

## 📖 API Documentation (Swagger)

Interactive API documentation is available via Swagger UI once the containers are running.

- **Swagger UI:** [http://localhost:8080/swagger](http://localhost:8080/swagger)

Use this interface to explore, test, and understand the available API endpoints.

---

## Notes

- The first build may take several minutes as Docker downloads images and installs dependencies.
- Ensure ports 3000, 3001, and 1433 are available on your machine.
- The database is ephemeral by default. To persist data, add a Docker volume to the `db` service in `docker-compose.yml`.

---

## Troubleshooting

- If the API cannot connect to the database, ensure the `db` container is healthy and the connection string matches.
