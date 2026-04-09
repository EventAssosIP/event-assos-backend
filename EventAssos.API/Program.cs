using EventAssos.API.Extentions;
using EventAssos.API.Scalar;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Application.Services;
using EventAssos.Infrastructure;
using EventAssos.Infrastructure.DataBase;
using EventAssos.Infrastructure.DataBase.Context;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// Lire config SMTP
// ----------------------------
var smtpConfig = builder.Configuration.GetSection("Smtp");

builder.Services.AddScoped<IEmailService>(sp =>
    new SmtpEmailService(
        smtpConfig["Host"]!,
        int.Parse(smtpConfig["Port"]!),
        smtpConfig["User"]!,
        smtpConfig["Pass"]!,
        smtpConfig["FromEmail"]!,
        smtpConfig["FromName"]!
    )
);

// ----------------------------
// Services Core & Infrastructure
// ----------------------------
builder.Services.ConfigureCore();
builder.Services.ConfigureInfrastructure(builder.Configuration);

// ----------------------------
// CORS - Dev local (tout ouvert)
// ----------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ----------------------------
// JWT Authentication & Authorization
// ----------------------------
builder.Services.ConfigureJwTAuthentication(builder.Configuration);
builder.Services.AddAuthorization(options =>
{
    // Le nom ici DOIT être identique à celui dans l'attribut [Authorize]
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// ----------------------------
// Controllers & OpenAPI / Scalar
// ----------------------------
builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

// ----------------------------
// Build App
// ----------------------------
var app = builder.Build();

// ----------------------------
// Dev only
// ----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();          // Affiche les exceptions détaillées
    app.MapOpenApi();                         // Swagger
    app.MapScalarApiReference();              // Scalar UI
}

// ----------------------------
// Middleware
// ----------------------------
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");                  // CORS
app.UseAuthentication();                    // Authentification JWT
app.UseAuthorization();                     // Autorisation

// ----------------------------
// Routes
// ----------------------------
app.MapControllers();

// ---------------------------------------------------------
// Seeding (Exécuté une seule fois au démarrage)
// ---------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EventAssosContext>();
        // On appelle ton initialiseur que tu as placé dans Infrastructure
        DbInitializer.Seed(context);
    }
    catch (Exception ex)
    {
        // On récupère le logger par défaut de .NET pour voir l'erreur en console
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Une erreur est survenue lors du remplissage initial (Seed) de la base de données.");
    }
}

// ---------------------------------------------------------
// Run
// ---------------------------------------------------------
app.Run();
