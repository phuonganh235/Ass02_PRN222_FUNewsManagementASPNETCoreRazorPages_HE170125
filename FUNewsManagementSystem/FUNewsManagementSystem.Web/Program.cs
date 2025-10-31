using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages
builder.Services.AddRazorPages();

// Session
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".FUNews.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// DbContext
builder.Services.AddDbContext<FUNewsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FUNewsManagement"));
});

// DI services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<ICommentService, CommentService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // must be before UseAuthorization if any

app.MapRazorPages();

app.Run();
