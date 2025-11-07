using JsonWebTokenwithIdentity.Data;
using JsonWebTokenwithIdentity.Interfaces;
using JsonWebTokenwithIdentity.Models;
using JsonWebTokenwithIdentity.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JsonWebTokenwithIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AppDbContext _context;
     
        private readonly UserManager<ApplicationUser> _userManager;
     
        private readonly RoleManager<IdentityRole> _roleManager;
        
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ITokenServices _tokenServices;
        
        public AccountsController(AppDbContext context,UserManager<ApplicationUser>userManager,
            ITokenServices tokenServices,RoleManager<IdentityRole>roleManager,SignInManager<ApplicationUser>signInManager)
        {

           _context = context;
           _userManager=userManager;
           _roleManager=roleManager;
           _signInManager=signInManager;
            _tokenServices = tokenServices;
        }
        [HttpPost("LogIn")]
        public async Task<ActionResult> Login([FromBody] loginViewModel model)
        {
            if (ModelState.IsValid) {
            return BadRequest(ModelState);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) {
                return Unauthorized(new { message = "Invalid Email,User Name or Password" });
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) {
                return Unauthorized(new { message = "Invalid Email,User Name or Password" });
            }
            //HttpContent.Session.SetString("userName", user.UserName);
            var token = _tokenServices.CreateToken(user);
            return Ok(
                new {
                    message="Login SuccessFull",
                    username=user.UserName,
                    token=token
                });
        }

        


    }
}
