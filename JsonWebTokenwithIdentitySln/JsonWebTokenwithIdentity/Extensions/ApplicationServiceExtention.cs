using JsonWebTokenwithIdentity.Data;
using JsonWebTokenwithIdentity.Interfaces;
using JsonWebTokenwithIdentity.Models;
using JsonWebTokenwithIdentity.Services;
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
            services.AddControllers();

            //configure Session Options:
            services.AddDistributedMemoryCache();
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


            services.AddCors();

            //services:
            services.AddScoped<ITokenServices, TokenService>();

            //Ensure the JWT is valid
            var secretKey = config["AppSettings: TokenKey"];
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
