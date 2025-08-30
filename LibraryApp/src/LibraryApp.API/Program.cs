using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryApp.Application.Books.Commands;
using LibraryApp.Application.Books.Queries;
using LibraryApp.Application.Common.Interfaces;
using LibraryApp.Infrastructure.Persistence;
using LibraryApp.src.LibraryApp.Application.Books.Commands;
using LibraryApp.src.LibraryApp.Application.Common.Behaviors;
using LibraryApp.src.LibraryApp.Infrastructure.Persistence.LibraryApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateBookCommand>());
// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateBookCommand>());

// Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookCommandValidator>();

// Register Validation Behavior in MediatR pipeline
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await SeedData.Initialize(dbContext);
}
// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

// API Endpoints
app.MapPost("/books", async (CreateBookCommand command, IMediator mediator) =>
{
    var id = await mediator.Send(command);
    return Results.Created($"/books/{id}", id);
});

app.MapGet("/books", async (IMediator mediator) =>
{
    var books = await mediator.Send(new GetBooksQuery());
    return Results.Ok(books);
});

app.MapDelete("/api/books/{id}", async (Guid id, IMediator mediator) =>
{
    await mediator.Send(new DeleteBookCommand(id));
    return Results.NoContent();
});

app.MapPut("/books/{id}/borrow", async (Guid id, IMediator mediator) =>
{
    await mediator.Send(new BorrowBookCommand(id));
    return Results.Ok(new { Message = $"Book {id} borrowed successfully" });
});

app.MapPut("/books/{id}/return", async (Guid id, IMediator mediator) =>
{
    await mediator.Send(new ReturnBookCommand(id));
    return Results.Ok(new { Message = $"Book {id} returned successfully" });
});

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (error is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                Errors = validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            });
        }
    });
});


app.Run();
