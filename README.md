
# Blogging Platform API

A RESTful API built with ASP.NET Core 8 to manage a blogging platform. It supports CRUD operations for blogs and their associated tags.

## Inspiration
This project was inspired by the [Backend Roadmap](https://roadmap.sh/projects/blogging-platform-api), which provides a comprehensive guide for building a blogging platform API.

## Features
- Get all blogs with search functionality.
- Get a single blog by ID.
- Create, update, and delete blogs.
- Eagerly loads related tags.

## Prerequisites
- .NET 8 SDK or later
- Entity Framework Core
- SQL Server

## How to Run
1. Clone the repository:
   ```bash
   git clone https://github.com/synchoz/blogging-platform-api.git
   cd BloggingPlatformAPI
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Update the database connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=BloggingPlatform;Trusted_Connection=True;"
   }
   ```

4. Apply migrations and update the database:
   ```bash
   dotnet ef database update
   ```

5. Run the application:
   ```bash
   dotnet run
   ```

6. Access the API at `https://localhost:5001/api/Blog`.

## API Endpoints
### Get All Blogs
`GET /api/Blog?term=<search-term>`

### Get a Blog by ID
`GET /api/Blog/{id}`

### Create a Blog
`POST /api/Blog`

**Request Body:**
```json
{
  "title": "Sample Blog",
  "content": "This is a sample blog post.",
  "category": "Tech",
  "tags": [
    { "tag": "ASP.NET" },
    { "tag": "C#" }
  ]
}
```

### Update a Blog
`PUT /api/Blog/{id}`

### Delete a Blog
`DELETE /api/Blog/{id}`
