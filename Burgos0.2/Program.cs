using Dal;
using DAL;
using Services;
using Web.DAL;
using Web.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ConexionBD>();

builder.Services.AddScoped<CarritoDAL>();
builder.Services.AddScoped<CarritoService>();
builder.Services.AddScoped<CategoriaDAL>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<CompraDAL>();
builder.Services.AddScoped<CompraService>();
builder.Services.AddScoped<CarritoService>();
builder.Services.AddScoped<DashboardDAL>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<InventarioDAL>();
builder.Services.AddScoped<InventarioService>();
builder.Services.AddScoped<OrdenDAL>();
builder.Services.AddScoped<OrdenService>();

builder.Services.AddScoped<KardexService>();

builder.Services.AddScoped<ProductoDAL>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ProveedorDAL>();
builder.Services.AddScoped<ProveedorService>();
builder.Services.AddScoped<RolDAL>();
builder.Services.AddScoped<RolService>();
builder.Services.AddScoped<UsuarioDAL>();
builder.Services.AddScoped<UsuarioService>();


var app = builder.Build();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
