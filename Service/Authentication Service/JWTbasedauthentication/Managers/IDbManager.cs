using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JWTbasedauthentication.Managers
{
    public interface IDbManager
    {
        DataTable FetchDataWithSP(IConfiguration configuration, string query, List<MySqlParameter> mySqlParameters);
    }
}
