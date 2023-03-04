using System;
using RegForm.Models;
using System.Data;
using System.Data.SqlClient;
using RegForm.Interfaces;

namespace RegForm.Services
{
    public class BaseConnection : IConnection
    {
        private readonly IConfiguration _configuration;
        public BaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection dbConnection()
        {
            string conn = Convert.ToString(_configuration.GetSection("MyConn").Value);
            IDbConnection idbconnection = new SqlConnection(conn);
            return idbconnection;
        }
    }
}

