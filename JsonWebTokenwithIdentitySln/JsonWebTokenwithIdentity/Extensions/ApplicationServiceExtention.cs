using FluentValidation;
using FluentValidation.AspNetCore;
using JsonWebTokenwithIdentity.Data;
using JsonWebTokenwithIdentity.DBIbitializer;
using JsonWebTokenwithIdentity.Interfaces;
using JsonWebTokenwithIdentity.Models;
using JsonWebTokenwithIdentity.Models.ViewModels;
using JsonWebTokenwithIdentity.Services;
using JsonWebTokenwithIdentity.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;

namespace JsonWebTokenwithIdentity.Extensions
{
    public static class ApplicationServiceExtention
    {
        public static IServiceCollection ApplicationService(this IServiceCollection services, IConfiguration config)
        {   
            //context connection:

            var connectionString = config.GetConnectionString("con")?? throw new InvalidOperationException("connection string 'con' not found");
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(config.GetConnectionString("con")));
           

            //configure Session Options:
            services.AddDistributedMemoryCache();

            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30);
           
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllers();
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
            services.AddScoped<IValidator<RegisterViewModel>,RegisterRequestValidator>();
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            services.AddScoped<IDbInitializer, JsonWebTokenwithIdentity.DBIbitializer.DbInitializer>();
            
            services.AddCors();

            //services:
            services.AddScoped<ITokenServices, TokenService>();
         

            //Ensure the JWT is valid
            var secretKey = config["AppSettings:TokenKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("JWT secret token is missing from the configuration");
            }
            //Configure jwt authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                op => {
                    op.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey=true,
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                        ValidateIssuer=false,
                        ValidateAudience=false

                    };
                
                });
                

            return services;
        }
    }
}
