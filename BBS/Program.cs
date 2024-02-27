
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
//app.MapControllerRoute(
//    name: "Dashboard",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//    );
app.MapControllerRoute(
    name: "dafault",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );





app.Run();
