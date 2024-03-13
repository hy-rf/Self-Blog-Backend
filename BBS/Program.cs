using BBS.Interfaces;
using BBS.Data;
using BBS.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "BBS",
            ValidAudience = "https://localhost:7064",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretkeyBBStetKsekBSreteySecret"))
        };
    });


builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("LocalDB"));
});



builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.IsEssential = true;
});


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IReplyService, ReplyService>();

var app = builder.Build();

app.UseSession();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );
app.Run();