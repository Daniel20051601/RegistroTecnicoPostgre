using Microsoft.EntityFrameworkCore;
using RegistroTecnicoPostgre.Components;
using RegistroTecnicoPostgre.DAL;
using RegistroTecnicoPostgre.Services;

// No se necesita 'using Microsoft.AspNetCore.Components.Web;' para esta configuración.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Si tu componente Tecnico/Index.razor usa @rendermode InteractiveServer

// No se agregan aquí los servicios para PersistentComponentState

var conStr = builder.Configuration.GetConnectionString("DefaultConnection");


// Configura el contexto de Entity Framework Core para PostgreSQL
builder.Services.AddDbContextFactory<Contexto>(options =>
{
    options.UseNpgsql(conStr, npgsqlOptions =>
    {
        // Habilitar la estrategia de reintento para errores transitorios de la base de datos
        // Configurado para 5 reintentos, con un retardo máximo de 30 segundos entre ellos.
        // Esto ayuda a manejar los 'TimeoutException' con Supabase.
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null // Usa los códigos de error predeterminados para reintentos de Npgsql
        );
    });
});



// Inyección de dependencias para tu servicio de técnicos
builder.Services.AddScoped<TecnicoServices>();

var app = builder.Build();

// Configure the HTTP request pipeline (middlewares).
if (!app.Environment.IsDevelopment())
{
    // Manejo de errores en producción
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // HSTS (HTTP Strict Transport Security) para forzar HTTPS
    app.UseHsts();
}

app.UseHttpsRedirection(); // Redirige de HTTP a HTTPS
app.UseStaticFiles();      // Sirve archivos estáticos como CSS, JavaScript, imágenes
app.UseRouting();          // Habilita el enrutamiento para endpoints de ASP.NET Core

app.UseAntiforgery(); // Protección contra ataques de falsificación de solicitudes entre sitios (CSRF)

// ** Opcional pero recomendado para aplicar migraciones automáticamente al inicio **
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<Contexto>();
//     try
//     {
//         dbContext.Database.Migrate(); // Aplica las migraciones pendientes
//     }
//     catch (Exception ex)
//     {
//         var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
//         logger.LogError(ex, "An error occurred while migrating the database.");
//     }
// }

// Mapea los activos estáticos de Blazor (generados al compilar)
app.MapStaticAssets();
// Mapea los componentes Razor y configura el modo de renderizado interactivo del servidor
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); // Esto permite que los componentes se ejecuten de forma interactiva en el servidor

app.Run(); // Ejecuta la aplicación