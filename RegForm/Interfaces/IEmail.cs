using System;
using RegForm.Models;

namespace RegForm.Interfaces
{
    public interface IEmail
    {
        void SendEmail(EmailRequest EmailBody);
    }
}

