using Application.Interfaces;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using FluentValidation;
using Hangfire;
using Hangfire.MemoryStorage;
using Infrastructure.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using Tudu.Application.Interfaces;
using Tudu.Application.Services;
using Tudu.Application.Validation;
using Tudu.Application.Validators;
using Tudu.Infrastructure.Context;
using Tudu.Infrastructure.Repositories;
using Tudu.Infrastructure.Services;
using Tudu.Ui.Components;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
       .AddEnvironmentVariables();

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                       ?? builder.Configuration.GetConnectionString("TuduConnection");

builder.Services.AddDbContext<TuduDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGridSettings"));

// Blazor & UI services
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSweetAlert2(options => { options.Theme = SweetAlertTheme.Dark; });

// Application services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserTaskService, UserTaskService>();
builder.Services.AddScoped<IEmailService, SendGridEmailService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddHostedService<ReminderBackgroundService>();

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
builder.Services.AddScoped<TaskCreateDtoValidator>();
builder.Services.AddScoped<TaskUpdateDtoValidator>();

// Authentication
const string AuthScheme = "tudu-auth";
const string AuthCookie = "tudu-cookie";

builder.Services.AddAuthentication(AuthScheme)
    .AddCookie(AuthScheme, options =>
    {
        options.Cookie.Name = AuthCookie;
        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/auth/access-denied";
        options.LogoutPath = "/auth/logout";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

builder.Services.AddHangfire(cfg => cfg.UseMemoryStorage());

var app = builder.Build();

/// -----------------------
/// Ensure Database is Ready
/// -----------------------
using (var scope = app.Services.CreateScope())
    {
    var db = scope.ServiceProvider.GetRequiredService<TuduDbContext>();
    int retries = 10;

    while (true)
        {
        try
            {
            db.Database.Migrate();
            Console.WriteLine("Database migration successful.");
            break;
            }
        catch (Exception ex)
            {
            retries--;
            if (retries == 0) throw;
            Console.WriteLine($"Database not ready yet: {ex.Message}. Retrying in 5s...");
            Thread.Sleep(5000);
            }
        }
    }

// -----------------------
// Ensure uploads folder exists
// -----------------------
var uploadsDir = Path.Combine(app.Environment.WebRootPath, "uploads");
if (!Directory.Exists(uploadsDir))
    {
    Directory.CreateDirectory(uploadsDir);
    Console.WriteLine($"Created uploads folder at {uploadsDir}");
    }

// -----------------------
// Middleware
// -----------------------
if (!app.Environment.IsDevelopment())
    {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    }

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// -----------------------
// Map Razor Components
// -----------------------
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
