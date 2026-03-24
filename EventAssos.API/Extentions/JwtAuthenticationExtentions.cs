using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventAssos.API.Extentions
{
    public static class JwtAuthenticationExtensions
    {
        public static void ConfigureJwTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Récupération de la section "JwtSettings" du fichier de configuration
            var jwtSettings = configuration.GetSection("JwtSettings");

            // Configuration de l'authentification JWT
            services
                // Configuration du système d'authentification dans le pipeline JWT et injecte le service d'authentification
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                // Configuration du JWT Bearer
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true, // Valide l'émetteur du token (qui a créé le token)
                        ValidateAudience = true, // Valide le destinataire du token (à qui le token est destiné)
                        ValidateLifetime = true, // Valide la durée de vie du token (assure que le token n'est pas expiré)
                        ValidateIssuerSigningKey = true, // Valide la clé de signature de l'émetteur du token (assure que le token n'a pas été altéré)

                        ValidIssuer = jwtSettings["Issuer"], // Émetteur attendu du token
                        ValidAudience = jwtSettings["Audience"], // Destinataire attendu du token
                        IssuerSigningKey = new SymmetricSecurityKey( // Clef de chiffrement symétrique utilisée pour valider la signature du token (la même clef pour signer ET vérifier)
                            Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!) // Conversion de la chaîne de caractères en tableau d'octets (UTF8)
                        ),
                        ClockSkew = TimeSpan.Zero // Pas de tolérance sur l'expiration du token (par défaut: 5 minutes)
                    };
                });
        }
    }
}
