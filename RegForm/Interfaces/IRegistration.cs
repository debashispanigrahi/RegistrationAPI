using System;
using RegForm.Models;

namespace RegForm.Interfaces
{
    public interface IRegistration
    {
        ResponseModel Registration(RegFormModel regFormModel);
    }
}

