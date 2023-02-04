using IdentityByExamples.EmailService;
using IdentityByExamples.Factory;
using IdentityByExamples.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));

builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 7;
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;

    opt.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomClaimsFactory>();

var emailConfig = builder.Services.AddOptions<EmailOptions>().BindConfiguration("EmailConfiguration");
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Add services to the container.
// builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// app.MapRazorPages();
app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
