using Application.Interfaces;
using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// In a real world scenario, you could replace this connection string for an actual database like an AWS RDS cluster.
// The connection string would be better stored in a json file but I added it here as it needs to be dynamically generated.
string databaseLocation =
    @$"{Directory.GetParent(Directory.GetCurrentDirectory())}\Infrastructure\Database\Database.mdf";
string connectionString =
    @$"Server=(localdb)\MSSQLLocalDB;AttachDbFilename={databaseLocation};Integrated Security=True;Connect Timeout=30";
builder.Services.AddDbContext<AppDatabaseContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IMeterReadingService, MeterReadingService>();

// This could be stricter to restrict to just the webui project localhost and port but I think this is good for a test solution.
// Typically you might not need this in a real world environment if frontend and backend are on the same domain. 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});


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

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
