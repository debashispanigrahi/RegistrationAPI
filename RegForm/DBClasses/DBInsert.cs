using System;
using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
            dbConnection = _dbConnection.dbConnection();
            dbConnection.Open();
            ResponseModel responseModel = dbConnection.Query<ResponseModel>("dbo.InsertUser", new
            {
                FirstName = regFormModel.firstName,
                LastName = regFormModel.lastName,
                Email = regFormModel.email,
                Mobile = regFormModel.mobile,
                Address = regFormModel.address,
                State = regFormModel.state,
                City = regFormModel.city,
                ZipCode = regFormModel.zipCode,
                CreatedAt = DateTime.Now
            }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            dbConnection.Close();
            return responseModel;
        }

        public Int64 InsertRequestLog(string Method, string ApiUrl, string queryString, string Body)
        {
            queryString = queryString.Substring(queryString.LastIndexOf('?') + 1);
            dbConnection = _dbConnection.dbConnection();
            dbConnection.Open();
            Int64 response = dbConnection.Query<Int64>("[dbo].[InsertRequestLog]", new
            {
                Type = Method,
                Url = ApiUrl,
                QueryString = queryString,
                RequestBody = Body,
                RequestTime = DateTime.Now
            }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            dbConnection.Close();
            return response;
        }

        public bool UpdateResponseLog(Int64 ApiLogId, int StatusCode = 0, string Response = "")
        {
            dbConnection = _dbConnection.dbConnection();
            dbConnection.Open();
            bool response = dbConnection.Query<bool>("[dbo].[UpdateResponseLog]", new
            {
                Id = ApiLogId,
                ResponseStatus = StatusCode,
                ResponseBody = Response,
                ResonseTime = DateTime.Now
            }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            dbConnection.Close();
            return response;
        }

        public bool InsertErrorLog(Int64 ApiLogId, string ApiUrl, string Message, string StackTrace)
        {
            dbConnection = _dbConnection.dbConnection();
            dbConnection.Open();
            bool response = dbConnection.Query<bool>("[dbo].[InsertErrorLog]", new
            {
                ApiLogId = ApiLogId,
                Url = ApiUrl,
                ExceptionMessage = Message,
                Stacktrace = StackTrace,
                ExceptionTime = DateTime.Now
            }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            dbConnection.Close();
            return response;
        }
    }
}

