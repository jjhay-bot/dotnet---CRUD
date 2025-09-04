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
    var todo = new Todo(nextId++, request.Title, request.IsComplete, request.Priority);
    todos.Add(todo);
    return Results.Created($"/todos/{todo.Id}", todo);
});

// Get all Todos
app.MapGet("/todos", () => todos);

// Get Todo by ID
app.MapGet("/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});

// Update Todo
app.MapPut("/todos/{id}", (int id, TodoCreateRequest request) =>
{
    var index = todos.FindIndex(t => t.Id == id);
    if (index == -1)
        return Results.NotFound();
    
    todos[index] = new Todo(id, request.Title, request.IsComplete, request.Priority);
    return Results.Ok(todos[index]);
});

// Delete Todo
app.MapDelete("/todos/{id}", (int id) =>
{
    var index = todos.FindIndex(t => t.Id == id);
    if (index == -1)
        return Results.NotFound();
    
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

record Todo(int Id, string Title, bool IsComplete, Priority Priority);

record TodoCreateRequest(string Title, bool IsComplete, Priority Priority);
