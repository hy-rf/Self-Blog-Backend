using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.SignalR;


using BBS.Data;
using BBS.IService;
using BBS.Services;
using BBS.Hubs;
using BBS.Common;
using BBS.IRepository;
using BBS.Repository;
using BBS.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey =true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT")!))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                ctx.Token = ctx.Request.Cookies["Token"];
                return Task.CompletedTask;
            }
        };
    });



builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("LocalDB"));
});


builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IReplyService, ReplyService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IFriendService, FriendService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<INotificationService, NotificationService>();



builder.Services.AddSingleton<IUserIdProvider, MyUserIdProvider>();

var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapHub<BBS.Hubs.Notification>("/notification");
app.MapHub<BBS.Hubs.ChatRoom>("/chat");


app.Run();