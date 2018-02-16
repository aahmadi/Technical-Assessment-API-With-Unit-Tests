using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Entities;
using Ali.Planning.API.Filters;
using API.Model;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ali.Planning.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("Any")]
    public class AuthController : Controller
    {
        private ILogger<AuthController> _logger;
        private UserManager<PlanningUser> _userMgr;
        private IPasswordHasher<PlanningUser> _hasher;
        private IConfiguration _config;

        public AuthController(
            ILogger<AuthController> logger,
            UserManager<PlanningUser> userManager,
            IConfiguration config
            )
        {
            _logger = logger;
            _userMgr = userManager;
            _config = config;
        }

        

        [HttpPost("token")]
        [ValidateModel]
        public async Task<IActionResult> CreateToken([FromBody] LoginCredentialsModel model)
        {
            try
            {
                var user = await _userMgr.FindByNameAsync(model.Username);
                if (user != null)
                {
                    var pass = await _userMgr.CheckPasswordAsync(user, model.Password);
                    if (/*_hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success*/ pass)
                    {
                        var userClaims = await _userMgr.GetClaimsAsync(user);

                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email)
                        }.Union(userClaims);
                        
                        var key = _config.GetSection("Tokens:key").Value;
                        var iss = _config.GetSection("Tokens:Issuer").Value;
                        var aud = _config.GetSection("Tokens:Audience").Value;

                        var symKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

                        var creds = new SigningCredentials(symKey, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                          issuer: iss,
                          audience: aud,
                          claims: claims,
                          expires: DateTime.UtcNow.AddDays(1),
                          signingCredentials: creds
                          );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }
                return BadRequest("Invalid credentials.");

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception thrown while creating JWT: {e}");
                ///TODO Handle exception
                return BadRequest("Failed to generate token");
            }
        }
    }
}
