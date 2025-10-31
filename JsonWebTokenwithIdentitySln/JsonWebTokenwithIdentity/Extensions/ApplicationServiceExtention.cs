using JsonWebTokenwithIdentity.Data;
using JsonWebTokenwithIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JsonWebTokenwithIdentity.Extensions
{
    public static class ApplicationServiceExtention
    {
        public static IServiceCollection ApplicationService(this IServiceCollection services, IConfiguration config)
        {
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
            return services;
        }
    }
}
