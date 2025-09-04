using Application.Interfaces;
using Blazored.LocalStorage;
using CurrieTechnologies.Razor.SweetAlert2;
//using Tudu.Application.Validations;
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

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);


builder.Services.AddDbContext<TuduDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TuduConnection")));
// "TuduConnection": "Server=(localdb)\\mssqllocaldb;Database=TuduDb;Trusted_Connection=True;"


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSweetAlert2(options => {
    options.Theme = SweetAlertTheme.Dark;
});

builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IUserTaskService, UserTaskService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

builder.Services.AddAuthentication().AddCookie();

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

builder.Services.AddScoped<AuthenticationStateProvider,
    ServerAuthenticationStateProvider>();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<CreateUserTaskRequestValidator>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserTaskService, UserTaskService>();
builder.Services.AddScoped<TaskCreateDtoValidator>();
builder.Services.AddScoped<TaskUpdateDtoValidator>();
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, SendGridEmailService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddHostedService<ReminderBackgroundService>();

builder.Services.AddHangfire(cfg =>
{
    // For dev:
    cfg.UseMemoryStorage();

    // For production prefer:
    // cfg.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("Hangfire"));
    // or cfg.UseRedisStorage(...);
});
//builder.Services.AddHangfireServer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    {
    var db = scope.ServiceProvider.GetRequiredService<TuduDbContext>();
    db.Database.Migrate();
    }

if (!app.Environment.IsDevelopment())
    {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    }

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();


//app.UseHangfireDashboard("/jobs", new DashboardOptions
//    {
//    Authorization = new[] { new HangfireDashboardAuthorizationFilter() } // implement filter to restrict access
//    });


app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
