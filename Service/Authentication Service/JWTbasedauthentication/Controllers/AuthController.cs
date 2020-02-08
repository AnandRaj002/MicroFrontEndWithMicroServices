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
using Microsoft.Extensions.Configuration;
using JWTbasedauthentication.Managers;
using System.Diagnostics;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using JWTbasedauthentication.Models;

namespace JWTbasedauthentication.Controllers
{
    // This controller generate valid Jwt token incase of valid user for basic authentication
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private DatabaseManager dbManager = null;
        private DataTable dtVal = null;

        public AuthController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register(UserModel userModel)
        {
            if(this.RegisterUser(_configuration, userModel) > 0)
            {
                return Ok($"{userModel.UserName} registered sucessfully");
            }
            return BadRequest("Not able to register");
        }
                
        [HttpPost("login")]
        public IActionResult Login(IdentityModel identity)
        {
            if(this.CompareCredentials(identity.UserName, identity.Password))
            {
                var tokenStringResponse = this.GetAthorizeToken(identity.UserName);
                return Ok(tokenStringResponse);
            }
            return BadRequest("Wrong Request");
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
                if(this.CompareCredentials(usernameAndPwd[0], usernameAndPwd[1]))
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

        #region PrivateMethods

        private bool CompareCredentials(string requestUserName, string requestPassword)
        {
            List<MySqlParameter> mySqlParameters = new List<MySqlParameter>();

            mySqlParameters.Add(new MySqlParameter("userName", requestUserName));

            dbManager = new DatabaseManager();

            dtVal = dbManager.FetchDataWithSP(_configuration, "Login", mySqlParameters);

            if (dtVal.Rows.Count > 0)
            {
                if (requestUserName == dtVal.Rows[0]["user_name"].ToString() && this.VerifyHash(requestPassword, dtVal.Rows[0]["password"].ToString(), dtVal.Rows[0]["passwordsalt"].ToString()))
                {
                    return true;
                }                    
            }
            return false;
        }

        private string GetAthorizeToken(string userName)
        {
            var claimdata = new[] { new Claim(ClaimTypes.Name, userName)};
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdfsadfasdasdaasd"));
            var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    issuer: "testweb.com",
                    audience: "testweb.com",
                    expires: DateTime.Now.AddMinutes(4),
                    claims: claimdata,
                    signingCredentials: signInCred
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        private int RegisterUser(IConfiguration configuration, UserModel userModel)
        {
            List<MySqlParameter> mySqlParameters = new List<MySqlParameter>();
            byte[] passwordSalt = null;

            var passwordHash = this.CreateHash(userModel.Password, out passwordSalt);

            mySqlParameters.Add(new MySqlParameter("userName", userModel.UserName));
            mySqlParameters.Add(new MySqlParameter("firstName", userModel.FirstName));
            mySqlParameters.Add(new MySqlParameter("lastName", userModel.LastName));
            mySqlParameters.Add(new MySqlParameter("email", userModel.Email));
            mySqlParameters.Add(new MySqlParameter("password", passwordHash));
            mySqlParameters.Add(new MySqlParameter("passwordSalt", Convert.ToBase64String(passwordSalt)));

            dbManager = new DatabaseManager();

            var recordUpdated = dbManager.ExecureDataWithSP(configuration, "Register_User", mySqlParameters);

            return recordUpdated;
        }
                
        private string CreateHash(string password, out byte[] passwordSalt)
        {
            byte[] passwordHash = null;
            using (var hash = new System.Security.Cryptography.HMACSHA256()) 
            {
                passwordSalt = hash.Key;
                passwordHash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));       
            }            

            return Convert.ToBase64String(passwordHash);
        }

        private bool VerifyHash(string password, string hashPassword, string hashSalt)
        {
            byte[] passwordHash = null;
            using (var hash = new System.Security.Cryptography.HMACSHA256(Convert.FromBase64String(hashSalt)))
            {
                passwordHash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            if (Convert.ToBase64String(passwordHash) == hashPassword)
                return true;
            return false;
        }

        #endregion
    }
}