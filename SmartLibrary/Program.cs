using Microsoft.EntityFrameworkCore;
using SmartLibrary.Data;
using SmartLibrary.Interfaces;
using SmartLibrary.Services;
using SmartLibrary.Repositories;
using SmartLibrary.Models;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Add Swagger for API testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register EF Core database (InMemory example)
builder.Services.AddDbContext<LibraryContext>(options =>
{
    options.UseInMemoryDatabase("LibraryDb");
});

// Register Repositories (Services depend on these!)
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();

// Register Services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoanService, LoanService>();

// Enable CORS (needed for JS → API calls)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ============ SEED DATABASE WITH TEST DATA ============
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();

    // Only seed if database is empty
    if (!context.Users.Any())
    {
        // Add test users
        var student = new Student
        {
            Username = "student1",
            Password = "pass123",
            Name = "John Student",
            Email = "student@test.com",
            Role = "Student",
            Program = "Computer Science"
        };

        var faculty = new Faculty
        {
            Username = "faculty1",
            Password = "pass123",
            Name = "Prof Smith",
            Email = "faculty@test.com",
            Role = "Faculty",
            Department = "Mathematics"
        };

        context.Users.AddRange(student, faculty);
        context.SaveChanges();
    }

    if (!context.Books.Any())
    {
        // Add test books
        var book1 = new Book
        {
            Title = "Introduction to Programming",
            Author = "John Doe",
            ISBN = "978-1234567890",
            TotalCopies = 5,
            CopiesAvailable = 5
        };

        var book2 = new Book
        {
            Title = "Data Structures",
            Author = "Jane Smith",
            ISBN = "978-0987654321",
            TotalCopies = 3,
            CopiesAvailable = 3
        };

        context.Books.AddRange(book1, book2);
        context.SaveChanges();
    }
}
// ============ END SEED DATA ============

// ENABLE wwwroot (static HTML/JS/CSS files)
app.UseStaticFiles();

// Enable CORS
app.UseCors();

// Enable Swagger UI in Development (ONLY at /swagger path)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger"; // Explicitly set to /swagger only
    });
}

// Map API controllers
app.MapControllers();

// Redirect root to login page - MUST be last
app.MapGet("/", () => Results.Redirect("/login.html"));

app.Run();