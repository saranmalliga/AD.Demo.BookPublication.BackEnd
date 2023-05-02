using AD.Demo.BookPublication.Api.DependencyConfig;
using AD.Demo.BookPublication.Domain.AutoMapper;
using AD.Demo.BookPublication.SQL.Infrastructure.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BookManagementDbContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("BookMgmtCoreConnection")
));
builder.Services.AddDapper(builder.Configuration);
builder.Services.AddMapper();
builder.Services.AddRepositories();
builder.Services.AddDataServices();
builder.Services.AddElasticSearch(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "DevEnvPolicy",
                policy =>
                {
                    policy.AllowAnyOrigin().WithMethods("POST", "PUT", "DELETE", "GET");
                });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("DevEnvPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
