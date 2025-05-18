using Microsoft.EntityFrameworkCore;
using RegistroTecnicoPostgre.Components;
using RegistroTecnicoPostgre.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var conStr = builder.Configuration.GetConnectionString("DefaultConnection");

//Configura el contexto para PostgreSQL
builder.Services.AddDbContextFactory<Contexto>(options => options.UseNpgsql(conStr));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
