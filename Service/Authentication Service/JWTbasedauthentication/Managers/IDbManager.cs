using JWTbasedauthentication.Helper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace JWTbasedauthentication.Managers
{
    public interface IDbManager
    {
        DataTable FetchDataWithSP(AppSettings appSettings, string query, List<MySqlParameter> mySqlParameters);

        int ExecureDataWithSP(AppSettings appSettings, string query, List<MySqlParameter> mySqlParameters);
    }
}
