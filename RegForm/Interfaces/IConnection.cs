using System.Data;

namespace RegForm.Interfaces
{
    public interface IConnection
    {
        IDbConnection dbConnection();
    }
}