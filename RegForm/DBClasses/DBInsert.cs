using System;
using System.Data;
using Dapper;
using RegForm.Interfaces;
using RegForm.Models;

namespace RegForm.DBClasses
{
    public class DBInsert : IDBInsert
    {
        private readonly IConnection _dbConnection;
        private IDbConnection dbConnection;
        public DBInsert(IConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public ResponseModel Registration(RegFormModel regFormModel)
        {
            try
            {
                dbConnection = _dbConnection.dbConnection();
                ResponseModel responseModel = dbConnection.Query<ResponseModel>("dbo.InsertUser", new
                {
                    FirstName = regFormModel.firstName,
                    LastName = regFormModel.lastName,
                    Email = regFormModel.email,
                    Mobile = regFormModel.mobile,
                    Address = regFormModel.address,
                    State = regFormModel.state,
                    City = regFormModel.city,
                    ZipCode = regFormModel.zipCode
                }, commandType: CommandType.StoredProcedure).SingleOrDefault();
                return responseModel;
            }
            catch (Exception ex) { throw ex; }
            finally { dbConnection.Close(); }
        }
    }
}

