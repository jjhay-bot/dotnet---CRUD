# Todo API

A simple ASP.NET Core Web API project for managing todos with full CRUD operations.

## Features

- Full CRUD operations for Todo items
- RESTful API endpoints
- Built with .NET 9.0
- OpenAPI/Swagger documentation support
- In-memory data storage
- JSON enum serialization support
- Priority levels for todos (Low, Mid, High)

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Running the Application

1. Clone this repository
2. Navigate to the project directory
3. Run the application:

```bash
dotnet run
```

The API will be available at:

- HTTP: `http://localhost:5214`
- HTTPS: `https://localhost:7256`

### API Documentation

When running in development mode, you can access the OpenAPI documentation at the `/openapi` endpoint.

### API Endpoints

#### Todo Operations

- `GET /todos` - Get all todos
- `GET /todos/{id}` - Get a specific todo by ID
- `POST /todos` - Create a new todo
- `PUT /todos/{id}` - Update an existing todo (full update - all fields required)
- `PATCH /todos/{id}` - Partially update a todo (only send fields you want to change)
- `DELETE /todos/{id}` - Delete a todo

#### Sample Todo Object

**For creating a new todo (POST):**

```json
{
  "title": "Learn ASP.NET Core",
  "isComplete": false,
  "priority": "High"
}
```

**For partial updates (PATCH):**

```json
{
  "isComplete": true
}
```

**Priority Levels:** `Low`, `Mid`, `High`

#### Other Endpoints

- `GET /weatherforecast` - Sample weather forecast endpoint

## Error Handling

The API uses structured error responses with standardized error codes for better integration and debugging.

### Error Response Format

**Validation Errors (400 Bad Request):**

```json
{
  "message": "Validation failed",
  "errors": [
    {
      "message": "Title must be at least 6 characters long",
      "code": 1002,
      "field": "title"
    }
  ]
}
```

**Not Found Errors (404 Not Found):**

```json
{
  "message": "Todo with ID 999 not found",
  "code": 2001,
  "field": "id"
}
```

### Error Codes Reference

#### Validation Errors (1xxx)

- `1001` - TITLE_REQUIRED: Title field is required
- `1002` - TITLE_TOO_SHORT: Title must be at least 6 characters
- `1003` - TITLE_TOO_LONG: Title cannot exceed 200 characters
- `1004` - INVALID_PRIORITY: Priority must be Low, Mid, or High
- `1005` - TITLE_EMPTY: Title cannot be empty (PATCH operations)

#### Resource Errors (2xxx)

- `2001` - TODO_NOT_FOUND: Todo with specified ID does not exist

### Frontend Integration Example

```javascript
// Handle errors programmatically
fetch('/todos', { method: 'POST', body: JSON.stringify(todo) })
  .then(response => response.json())
  .then(data => {
    if (data.errors) {
      data.errors.forEach(error => {
        switch(error.code) {
          case 1001: // TITLE_REQUIRED
          case 1002: // TITLE_TOO_SHORT
            highlightField(error.field);
            break;
          case 2001: // TODO_NOT_FOUND
            showNotFoundMessage();
            break;
        }
      });
    }
  });
```

## Development

This project uses:

- ASP.NET Core 9.0
- Minimal APIs
- OpenAPI for documentation

## Project Structure

```text
TodoApi/
├── Program.cs              # Application entry point and configuration
├── TodoApi.csproj         # Project file
├── Properties/
│   └── launchSettings.json # Launch configuration
├── appsettings.json       # Application settings
└── appsettings.Development.json # Development settings
```

## Contributing

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License.
