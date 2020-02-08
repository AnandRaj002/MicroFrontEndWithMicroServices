using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JWTbasedauthentication.Managers
{
    public class DatabaseManager : IDbManager
    {
        //private readonly IConfiguration _configuration;

        //public DatabaseManager(IConfiguration configuration)
        //{
        //    this._configuration = configuration;
        //}

        public DataTable FetchDataWithSP(IConfiguration configuration, string query, List<MySqlParameter> mySqlParameters)
        {
            var connectionString = configuration.GetConnectionString("mySQLConnectionString");

            DataTable dtVal = new DataTable();

            using (MySqlConnection myConn = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter myAda = new MySqlDataAdapter(query, myConn))
                {
                    myAda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    
                    if(mySqlParameters != null)
                    {
                        mySqlParameters.ForEach(val => myAda.SelectCommand.Parameters.Add(val));
                    }
                    
                    myAda.Fill(dtVal);
                }
            }

            return dtVal;
        }

        public int ExecureDataWithSP(IConfiguration configuration, string query, List<MySqlParameter> mySqlParameters)
        {
            var connectionString = configuration.GetConnectionString("mySQLConnectionString");

            int result = 0;

            using (MySqlConnection myConn = new MySqlConnection(connectionString))
            {
                myConn.Open();
                using (MySqlCommand myCmd = new MySqlCommand(query, myConn))
                {
                    myCmd.CommandType = CommandType.StoredProcedure;

                    if (mySqlParameters != null)
                    {
                        mySqlParameters.ForEach(val => myCmd.Parameters.Add(val));
                    }

                   result = myCmd.ExecuteNonQuery();
                }
            }

            return result;
        }
    }
}
