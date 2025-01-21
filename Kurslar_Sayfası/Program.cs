using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options=>{
    options.LoginPath="/User/Login";
});


builder.Services.AddDbContext<BlobContext>(options=>{
   var config=builder.Configuration;
   var connectionString=config.GetConnectionString("sql_connection");
   
   options.UseSqlite(connectionString);

   /* mysql bğlantısı kurma
   var connectionString=config.GetConnectionString("mysql_connection");
   var version=new MySqlServerVersion(new Version(8,0,30));
   options.UseMySql(connectionString,version);*/
});
var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
SeedDatabase.verilerigetir(app);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name:"post_details",
    pattern:"posts/details/{url}",
    defaults:new {Controller="Post",action="Details"}
);

app.MapControllerRoute(
    name:"post_bay_tag",
    pattern:"posts/tag/{tagurl}",
    defaults:new {Controller="Post",action="Index"}
);

app.MapControllerRoute(
    name:"Profil_details",
    pattern:"user/Profil/{username}",
    defaults:new {Controller="User",action="Profil"}
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Post}/{action=Index}/{id?}");

app.Run();
