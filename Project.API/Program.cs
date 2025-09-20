using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MotorbikeRental.API.Extensions;
using Project.API.Extensions;
using Project.API.Middlewares;
using Project.Application.Interfaces.IDataSeedingServices;
using Project.Application.Validators.AuthValidators;
using Project.Common.Options;
using Project.Infrastructure.Data.Contexts;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("PrimaryDbConnection"));
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<AdminAccount>(builder.Configuration.GetSection("AdminAccount"));

builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

builder.Services.Register();
builder.Services.RegisterSecurityService(builder.Configuration);

builder.Services.AddCustomCors(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

builder.Services.AddCustomApiVersioning();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(LoginValidator).Assembly);

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddCustomSwagger();

var app = builder.Build();

#region Database Initialization
using var scope = app.Services.CreateScope();

var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await db.Database.MigrateAsync();

var seedingService = scope.ServiceProvider.GetRequiredService<IDataSeedingService>();
await seedingService.SeedDataAsync();
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DefaultModelsExpandDepth(-1);
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                    $"API {description.GroupName.ToUpperInvariant()}");
        }
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(CorsExtension.GetPolicyName());

#region Custom Middlewares
app.UseExceptionHandling();
app.UseRequestResponseLogging();
#endregion

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
