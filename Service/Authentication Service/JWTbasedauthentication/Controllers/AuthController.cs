using JWTbasedauthentication.Helper;
using JWTbasedauthentication.Managers;
using JWTbasedauthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTbasedauthentication.Controllers
{
    // This controller generate valid Jwt token incase of valid user for basic authentication
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppSettings _appSetting;
        private IUserManager _userManager;

        public AuthController(IOptions<AppSettings> appSettings, IUserManager userManager)
        {
            _appSetting = appSettings.Value;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public IActionResult Register(UserModel userModel)
        {
            try
            {
                if (_userManager.RegisterUser(_appSetting, userModel) > 0)
                {
                    return Ok($"{userModel.UserName} registered sucessfully");
                }

                return BadRequest("User Not Registered");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
                
        [HttpPost("login")]
        public IActionResult Login(IdentityModel identity)
        {
            try
            {
                if (_userManager.CompareCredentials(_appSetting, identity.UserName, identity.Password))
                {
                    var tokenStringResponse = _userManager.GetAthorizeToken(identity.UserName);
                    return Ok(tokenStringResponse);
                }
                return NotFound($"{identity.UserName} not registered with system.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("token")]
        public IActionResult Token()
        {            
            var header = Request.Headers["Authorization"];
            if(header.ToString().StartsWith("Basic"))
            {
                var credValue = header.ToString().Substring("Basic ".Length).Trim(); //Find credential from Request Header
                var usernameAndPwdValue = Encoding.UTF8.GetString(Convert.FromBase64String(credValue)); // Convert Encoded credential into strings
                var usernameAndPwd = usernameAndPwdValue.Split(":");

                // Verfiy Username and Password is valid or not use DB or any other methods
                if(_userManager.CompareCredentials(_appSetting, usernameAndPwd[0], usernameAndPwd[1]))
                {
                    // Create Claim Identity not mandatory which will check who can claim the token can use as role also
                    var claimdata = new[] { new Claim(ClaimTypes.Name, usernameAndPwd[0]) };

                    // Create Symmetric Security Key for signIn tokens
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdfsadfasdasdaasd"));

                    // Create signIn cred for symmetric key
                    var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                    // Create Token
                    var token = new JwtSecurityToken(
                            issuer: "testweb.com",
                            audience: "testweb.com",
                            expires: DateTime.Now.AddMinutes(4),
                            claims: claimdata,
                            signingCredentials: signInCred
                        );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(tokenString);
                }
            }
            return BadRequest("Wrong Request");
        }        
    }
}