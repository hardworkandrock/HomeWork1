using AccountService.App.Validators;
using AccountService.Data;
using AccountService.Infrastructure;
using AccountService.Infrastructure.Interface;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Service
builder.Services.AddControllers();

// DbContext
// builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IWalletStorageService, InMemoryWalletStorageService>();
builder.Services.AddSingleton<IClientVerificationService, InMemoryClientVerificationService>();
builder.Services.AddSingleton<ICurrencyService, InMemoryCurrencyService>();
builder.Services.AddSingleton<IOwnerService, InMemoryOwnerService>();

// MediatoR 
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Validation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "HomeWork1 API",
        Description = "API для управления счетами и транзакциями"
    });

    // Включаем XML-комментарии (если есть)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});


var app = builder.Build();
// Pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeWork1 API v1");
        c.RoutePrefix = "swagger"; // доступ по /swagger
    });
}

// ExceptionValidation
app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

    if (exception is ValidationException validationEx)
    {
        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";

        var errors = validationEx.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        await context.Response.WriteAsJsonAsync(new
        {
            error = "Validation failed",
            details = errors
        });
    }
    else
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { error = "Internal server error" });
    }
}));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/health", () => Results.Ok("OK"));

app.Run();
