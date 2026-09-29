using DimDim.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
// Lê APPLICATIONINSIGHTS_CONNECTION_STRING (App Setting do Web App)
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddDbContext<AppDb>(o => o.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    s => s.EnableRetryOnFailure()));

var app = builder.Build();

// Garante as tabelas caso o DDL (scripts/ddl.sql) ainda não tenha sido executado
using (var scope = app.Services.CreateScope())
{
    try { scope.ServiceProvider.GetRequiredService<AppDb>().Database.EnsureCreated(); }
    catch (Exception ex) { app.Logger.LogError(ex, "Falha ao conectar no Azure SQL"); }
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
