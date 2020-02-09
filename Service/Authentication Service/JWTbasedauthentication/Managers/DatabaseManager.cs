using JWTbasedauthentication.Helper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace JWTbasedauthentication.Managers
{
    public class DatabaseManager : IDbManager
    {
        public DataTable FetchDataWithSP(AppSettings appSettings, string query, List<MySqlParameter> mySqlParameters)
        {
            try
            {
                var connectionString = appSettings.mySQLConnectionString;

                DataTable dtVal = new DataTable();

                using (MySqlConnection myConn = new MySqlConnection(connectionString))
                {
                    using (MySqlDataAdapter myAda = new MySqlDataAdapter(query, myConn))
                    {
                        myAda.SelectCommand.CommandType = CommandType.StoredProcedure;

                        if (mySqlParameters != null)
                        {
                            mySqlParameters.ForEach(val => myAda.SelectCommand.Parameters.Add(val));
                        }

                        myAda.Fill(dtVal);
                    }
                }

                return dtVal;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int ExecureDataWithSP(AppSettings appSettings, string query, List<MySqlParameter> mySqlParameters)
        {
            try
            {
                var connectionString = appSettings.mySQLConnectionString;

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
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
