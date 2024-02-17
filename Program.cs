using Microsoft.EntityFrameworkCore;
using PassMngr.DBContext;
using PassMngr.Models;
using PassMngr.Repository;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Password>, PasswordRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("PassMngrPolicy",
        policy =>
        {
            policy.AllowAnyOrigin() //WithOrigins("[client url?]")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
                
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseCors(builder => builder.SetIsOriginAllowed((host) => true));
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseHttpsRedirection();
app.Run();

