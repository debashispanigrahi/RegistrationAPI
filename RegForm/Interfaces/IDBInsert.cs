using System;
using RegForm.Models;

namespace RegForm.Interfaces
{
    public interface IDBInsert
    {
        ResponseModel Registration(RegFormModel regFormModel);
        Int64 InsertRequestLog(string Method, string ApiUrl, string queryString, string Body);
        bool UpdateResponseLog(Int64 ApiLogId, int StatusCode, string Response);
        bool InsertErrorLog(Int64 ApiLogId, string ApiUrl, string Message, string StackTrace);
    }
}