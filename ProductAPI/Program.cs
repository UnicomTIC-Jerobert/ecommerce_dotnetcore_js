using Microsoft.Data.Sqlite;
using ProductAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Read the connection string from configuration
// Read the connection string from configuration
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register the repository with the connection string
builder.Services.AddScoped<IProductRepository>(provider => new ProductRepository(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

// Database creation
using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var command = new SqliteCommand(
        @"CREATE TABLE IF NOT EXISTS Products 
            (
            ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
            ProductName TEXT, 
            Price REAL, 
            Description TEXT,
            ImageURL TEXT
            )
            ", connection);
    command.ExecuteNonQuery();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
