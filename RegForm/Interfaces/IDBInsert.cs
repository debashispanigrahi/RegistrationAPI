using RegForm.Models;

namespace RegForm.Interfaces
{
    public interface IDBInsert
    {
        ResponseModel Registration(RegFormModel regFormModel);
    }
}