using EventAssos.API.Extentions;
using EventAssos.API.Scalar;

var builder = WebApplication.CreateBuilder(args);

// Configuration du Core et Infrastructure (méthodes d'extension)
builder.Services.ConfigureCore();
builder.Services.ConfigureInfrastructure(builder.Configuration);

// Configuration des cors
builder.Services.ConfigureCorsPolicy(builder.Configuration);

// Configuration de l'authentification JWT et de l'autorisation
builder.Services.ConfigureJwTAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy"); // Permet d'utiliser les CORS

app.UseAuthentication(); // Permet d'utiliser le pipeline d'authentification
app.UseAuthorization(); // Ordre important: UseAuthentication => UseAuthorization
// Vérification de qui est l'utilisateur, on vérifie ses droits avant de lui permettre d'accéder aux ressources

app.MapControllers();

app.Run();
