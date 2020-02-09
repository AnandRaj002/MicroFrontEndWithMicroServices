using JWTbasedauthentication.Helper;
using JWTbasedauthentication.Models;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTbasedauthentication.Managers
{
    public class UserManager : IUserManager
    {

        #region classVariables
        
        private DatabaseManager _dbManager;
        private DataTable _dtVal;
        private List<MySqlParameter> _mySqlParameters;
        private byte[] _passwordHash;
        private byte[] _passwordSalt;

        #endregion

        #region User Registration Methods

        public int RegisterUser(AppSettings appSettings, UserModel userModel)
        {
            _mySqlParameters = new List<MySqlParameter>();

            try
            {

                var passwordHash = CreateHash(userModel.Password, out _passwordSalt);

                _mySqlParameters.Add(new MySqlParameter("userName", userModel.UserName));
                _mySqlParameters.Add(new MySqlParameter("firstName", userModel.FirstName));
                _mySqlParameters.Add(new MySqlParameter("lastName", userModel.LastName));
                _mySqlParameters.Add(new MySqlParameter("email", userModel.Email));
                _mySqlParameters.Add(new MySqlParameter("password", passwordHash));
                _mySqlParameters.Add(new MySqlParameter("passwordSalt", Convert.ToBase64String(_passwordSalt)));

                _dbManager = new DatabaseManager();
                var recordUpdated = _dbManager.ExecureDataWithSP(appSettings, "Register_User", _mySqlParameters);

                return recordUpdated;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }        

        #endregion

        #region User Authentication Methods

        public bool CompareCredentials(AppSettings appSettings, string requestUserName, string requestPassword)
        {
            _mySqlParameters = new List<MySqlParameter>();
            _mySqlParameters.Add(new MySqlParameter("userName", requestUserName));

            try
            {
                _dbManager = new DatabaseManager();
                _dtVal = _dbManager.FetchDataWithSP(appSettings, "Login", _mySqlParameters);

                if (_dtVal.Rows.Count > 0)
                {
                    if (requestUserName == _dtVal.Rows[0]["user_name"].ToString() && VerifyHash(requestPassword, _dtVal.Rows[0]["password"].ToString(), _dtVal.Rows[0]["salt"].ToString()))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }        

        public string GetAthorizeToken(string userName)
        {
            try
            {
                var claimdata = new[] { new Claim(ClaimTypes.Name, userName) };
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
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Internal Private Methods

        private string CreateHash(string password, out byte[] passwordSalt)
        {
            try
            {
                using (var hash = new System.Security.Cryptography.HMACSHA256())
                {
                    passwordSalt = hash.Key;
                    _passwordHash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }

                return Convert.ToBase64String(_passwordHash);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool VerifyHash(string password, string hashPassword, string hashSalt)
        {
            try
            {
                using (var hash = new System.Security.Cryptography.HMACSHA256(Convert.FromBase64String(hashSalt)))
                {
                    _passwordHash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                }

                if (Convert.ToBase64String(_passwordHash) == hashPassword)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
