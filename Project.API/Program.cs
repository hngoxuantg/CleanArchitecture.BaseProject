using FluentValidation;
using FluentValidation.AspNetCore;
using Project.API.Extensions;
using Project.API.Middlewares;
using Project.Application.Mappers;
using Project.Application.Validators.AuthValidators;

var builder = WebApplication.CreateBuilder(args);

#region Database
builder.Services.AddCustomDb(builder.Configuration);
#endregion

#region Options
builder.Services.AddCustomOptions(builder.Configuration);
#endregion

#region Custom Services
builder.Services.Register();
builder.Services.RegisterSecurityService(builder.Configuration);
builder.Services.AddCustomCors(builder.Configuration);
builder.Services.AddCustomApiVersioning();
builder.Services.AddCustomSwagger();
#endregion

#region Framework Services
builder.Services.AddCustomControllers();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
#endregion

#region AutoMapper Services
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
#endregion

#region Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(LoginValidator).Assembly);
builder.Services.AddCustomFluentValidation();
#endregion

var app = builder.Build();

#region Database Initialization
await app.UseDatabaseInitialization();
#endregion

#region Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwaggerUI();
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
#endregion

app.Run();
