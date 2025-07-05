using FileService.Api.Middlewares;
using FileService.Api.Security;
using FileService.Application;
using FileService.Infrastructure;
using FileService.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.RegisterDataServices(builder.Configuration);
builder.Services.RegisterApplicationServices();
builder.Services.RegisterAuthServices(builder.Configuration);
builder.Services.RegisterSwaggerServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware(typeof(ErrorHandlingMiddleware));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

MigrateDbToLatestVersion(app);


await app.RunAsync();


static void MigrateDbToLatestVersion(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        context.Database.Migrate();
    }
    catch (SqlException ex) when (ex.Message.Contains("already exists"))
    {
        // Optional: log or skip
    }
}