using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTbasedauthentication.Controllers
{
    // This controller generate valid Jwt token incase of valid user for basic authentication
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
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
                if(usernameAndPwd[0] == "Admin" && usernameAndPwd[1] == "Password")
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