using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Api;

public class Startup {
    public IWebHostEnvironment Environment { get; }

    public void ConfigureServices(IServiceCollection services) {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options => {
                options.Authority = "http://localhost:5000/";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters =
                    new TokenValidationParameters {ValidateAudience = false};
            });

        services.AddAuthorization(options => {
            options.AddPolicy("ApiScope", policy => {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("APITest", "cinema");
            });
        });
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app) {
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers().RequireAuthorization("ApiScope");
        });
    }
}