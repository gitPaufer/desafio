using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Imo.Data;
using Imo.Areas.Identity.Data;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ImoContextConnection") ?? throw new InvalidOperationException("Connection string 'ImoContextConnection' not found.");

builder.Services.AddDbContext<ImoContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ImoUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ImoContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "image",
    pattern: "{controller=Imovel}/{action=Imagem}/{id?}");

app.MapControllerRoute(
    name: "customIndexImovel",
    pattern: "{controller=Imovel}/{action=IndexImovel}/{id?}");

app.MapRazorPages();
app.Run();
