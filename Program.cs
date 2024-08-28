using EFCoreDemo.Automapper;
using EFCoreDemo.Domain.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EFCoreDemoContext>(
p => p.UseNpgsql(builder.Configuration.GetConnectionString("EFCoreDemoConnnection")));


builder.Services.AddDbContext<EFCoreDemoContextAdvance>(
p => p.UseNpgsql(builder.Configuration.GetConnectionString("EFCoreDemoConnnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EFCoreDemoContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();