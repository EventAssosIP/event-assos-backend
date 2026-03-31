using EventAssos.API.Extentions;
using EventAssos.API.Scalar;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Application.Interfaces.Services.Tools;
using EventAssos.Application.Services;
using EventAssos.Infrastructure;
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
builder.Services.AddAuthorization();

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

// ----------------------------
// Run
// ----------------------------
app.Run();