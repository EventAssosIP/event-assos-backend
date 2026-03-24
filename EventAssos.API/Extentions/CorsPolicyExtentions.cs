namespace EventAssos.API.Extentions
{
    public static class CorsPolicyExtensions
    {
        public static void ConfigureCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            // Récupération des origines autorisées depuis la configuration
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                ?? throw new InvalidOperationException("Cors not configured.");

            // Configuration des CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {


                    // 3.  Configuration avec une liste d'origines autorisées depuis l'appsettings (recommandé)

                    policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                });
            });
        }
    }
}