var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure JSON options to handle enums as strings
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// In-memory list
var todos = new List<Todo>();
var nextId = 1;

// Create Todo
app.MapPost("/todos", (TodoCreateRequest request) =>
{
    // Enhanced validation with error codes
    var errors = new List<ErrorResponse>();
    
    if (string.IsNullOrWhiteSpace(request.Title))
        errors.Add(new ErrorResponse("Title is required", ErrorCode.TITLE_REQUIRED, "title"));
    
    if (request.Title?.Length < 6)
        errors.Add(new ErrorResponse("Title must be at least 6 characters long", ErrorCode.TITLE_TOO_SHORT, "title"));

    if (request.Title?.Length > 200)
        errors.Add(new ErrorResponse("Title cannot exceed 200 characters", ErrorCode.TITLE_TOO_LONG, "title"));
    
    if (!Enum.IsDefined(typeof(Priority), request.Priority))
        errors.Add(new ErrorResponse("Invalid priority value", ErrorCode.INVALID_PRIORITY, "priority"));
    
    if (errors.Any())
        return Results.BadRequest(new ValidationErrorResponse("Validation failed", errors));
    
    var todo = new Todo(nextId++, request.Title!.Trim(), request.IsComplete, request.Priority);
    todos.Add(todo);
    return Results.Created($"/todos/{todo.Id}", todo);
});

// Get all Todos
app.MapGet("/todos", () => todos);

// Get Todo by ID
app.MapGet("/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    return todo is not null ? Results.Ok(todo) : 
        Results.NotFound(new ErrorResponse($"Todo with ID {id} not found", ErrorCode.TODO_NOT_FOUND, "id"));
});

// Update Todo (full update)
app.MapPut("/todos/{id}", (int id, TodoCreateRequest request) =>
{
    var index = todos.FindIndex(t => t.Id == id);
    if (index == -1)
        return Results.NotFound(new ErrorResponse($"Todo with ID {id} not found", ErrorCode.TODO_NOT_FOUND, "id"));
    
    // Enhanced validation with error codes
    var errors = new List<ErrorResponse>();
    
    if (string.IsNullOrWhiteSpace(request.Title))
        errors.Add(new ErrorResponse("Title is required", ErrorCode.TITLE_REQUIRED, "title"));
    
    if (request.Title?.Length < 6)
        errors.Add(new ErrorResponse("Title must be at least 6 characters long", ErrorCode.TITLE_TOO_SHORT, "title"));

    if (request.Title?.Length > 200)
        errors.Add(new ErrorResponse("Title cannot exceed 200 characters", ErrorCode.TITLE_TOO_LONG, "title"));
    
    if (!Enum.IsDefined(typeof(Priority), request.Priority))
        errors.Add(new ErrorResponse("Invalid priority value", ErrorCode.INVALID_PRIORITY, "priority"));
    
    if (errors.Any())
        return Results.BadRequest(new ValidationErrorResponse("Validation failed", errors));
    
    todos[index] = new Todo(id, request.Title!.Trim(), request.IsComplete, request.Priority);
    return Results.Ok(todos[index]);
});

// Partial update Todo
app.MapPatch("/todos/{id}", (int id, TodoUpdateRequest request) =>
{
    var index = todos.FindIndex(t => t.Id == id);
    if (index == -1)
        return Results.NotFound(new ErrorResponse($"Todo with ID {id} not found", ErrorCode.TODO_NOT_FOUND, "id"));
    
    // Validation for partial update with error codes
    var errors = new List<ErrorResponse>();
    
    if (request.Title is not null && string.IsNullOrWhiteSpace(request.Title))
        errors.Add(new ErrorResponse("Title cannot be empty", ErrorCode.TITLE_EMPTY, "title"));
    
    if (request.Title is not null && request.Title.Length < 6)
        errors.Add(new ErrorResponse("Title must be at least 6 characters long", ErrorCode.TITLE_TOO_SHORT, "title"));

    if (request.Title?.Length > 200)
        errors.Add(new ErrorResponse("Title cannot exceed 200 characters", ErrorCode.TITLE_TOO_LONG, "title"));
    
    if (request.Priority is not null && !Enum.IsDefined(typeof(Priority), request.Priority))
        errors.Add(new ErrorResponse("Invalid priority value", ErrorCode.INVALID_PRIORITY, "priority"));
    
    if (errors.Any())
        return Results.BadRequest(new ValidationErrorResponse("Validation failed", errors));
    
    var existingTodo = todos[index];
    var updatedTodo = new Todo(
        id,
        request.Title?.Trim() ?? existingTodo.Title,
        request.IsComplete ?? existingTodo.IsComplete,
        request.Priority ?? existingTodo.Priority
    );
    
    todos[index] = updatedTodo;
    return Results.Ok(updatedTodo);
});

// Delete Todo
app.MapDelete("/todos/{id}", (int id) =>
{
    var index = todos.FindIndex(t => t.Id == id);
    if (index == -1)
        return Results.NotFound(new ErrorResponse($"Todo with ID {id} not found", ErrorCode.TODO_NOT_FOUND, "id"));
    
    todos.RemoveAt(index);
    return Results.NoContent();
});


app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC,
string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

enum Priority
{
    Low,
    Mid,
    High
}

enum ErrorCode
{
    TITLE_REQUIRED = 1001,
    TITLE_TOO_SHORT = 1002,
    TITLE_TOO_LONG = 1003,
    INVALID_PRIORITY = 1004,
    TODO_NOT_FOUND = 2001,
    TITLE_EMPTY = 1005
}

record ErrorResponse(string Message, ErrorCode Code, string? Field = null);

record ValidationErrorResponse(string Message, List<ErrorResponse> Errors);

record Todo(int Id, string Title, bool IsComplete, Priority Priority);

record TodoCreateRequest(string Title, bool IsComplete, Priority Priority);

record TodoUpdateRequest(string? Title, bool? IsComplete, Priority? Priority);
