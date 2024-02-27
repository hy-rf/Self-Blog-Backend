
using BBS.Data.Migrations;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "dafault",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );
app.Run();




var test = new Test();
using (var serviceProvider = test.CreateServices())
using (var scope = serviceProvider.CreateScope())
{
    // Put the database update into a scope to ensure
    // that all resources will be disposed.
    test.UpdateDatabase(scope.ServiceProvider);
}



/// <summary>
/// Update the database
/// </summary>

//app.MapControllerRoute(
//    name: "Dashboard",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//    );
