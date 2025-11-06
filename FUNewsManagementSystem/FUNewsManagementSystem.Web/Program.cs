using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Context;
using DataAccess.Repositories;
using FUNewsManagementSystem.Web.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// Configure Database Context
builder.Services.AddDbContext<FUNewsManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FUNewsManagement")));

// Register Repositories
builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<INewsTagRepository, NewsTagRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

// Register Services
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<IAccountService, AccountService>(); // Use AccountService class
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Configure Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<NotificationHub>("/notificationHub");

app.Run();