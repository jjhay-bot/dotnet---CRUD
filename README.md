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
- `PUT /todos/{id}` - Update an existing todo
- `DELETE /todos/{id}` - Delete a todo

#### Sample Todo Object

```json
{
  "title": "Learn ASP.NET Core",
  "isComplete": false,
  "priority": "High"
}
```

**Priority Levels:** `Low`, `Mid`, `High`

#### Other Endpoints

- `GET /weatherforecast` - Sample weather forecast endpoint

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
