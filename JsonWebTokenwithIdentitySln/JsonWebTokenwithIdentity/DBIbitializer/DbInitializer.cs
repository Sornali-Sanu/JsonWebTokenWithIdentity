using JsonWebTokenwithIdentity.Data;
using JsonWebTokenwithIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JsonWebTokenwithIdentity.DBIbitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public DbInitializer(AppDbContext context,UserManager<ApplicationUser>userManager,RoleManager<IdentityRole>roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            
        }
        public void Initialize()
        {

            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }

            }
            catch (Exception)
            {

                
            }
            if (_context.Roles.Any(x => x.Name == Utility.Helper.Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(Utility.Helper.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Utility.Helper.Doctor)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Utility.Helper.Patient)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName="admin@dami.com",
                Email= "admin@dami.com",
                EmailConfirmed= true,
                Name="Admin Sornali"

            },"Admin1234!").GetAwaiter().GetResult();

            ApplicationUser user=_context.Users.FirstOrDefault(x=>x.Email== "admin@dami.com");
            _userManager.AddToRoleAsync(user, Utility.Helper.Admin).GetAwaiter().GetResult();
        }
    }
}
