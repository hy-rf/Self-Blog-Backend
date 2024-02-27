
using BBS.Data.Migrations;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

var test = new Test();
using (var serviceProvider = test.CreateServices())
using (var scope = serviceProvider.CreateScope())
{
    test.UpdateDatabase(scope.ServiceProvider);
}
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "dafault",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );
app.Run();








/// <summary>
/// Update the database
/// </summary>

//app.MapControllerRoute(
//    name: "Dashboard",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//    );
