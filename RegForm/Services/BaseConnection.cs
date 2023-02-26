using System;
using RegForm.Models;
using System.Data;
using System.Data.SqlClient;
using RegForm.Interfaces;

namespace RegForm.Services
{
    public class BaseConnection : IConnection
    {
        IDbConnection idbconnection;

        public IDbConnection dbConnection()
        {
            string conn = Convert.ToString(ConfigManager.Appsettings.GetSection("MyConn").Value);
            idbconnection = new SqlConnection(conn);
            return idbconnection;
        }
    }
}

