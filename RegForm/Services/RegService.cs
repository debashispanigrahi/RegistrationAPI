using System;
using RegForm.DBClasses;
using RegForm.Interfaces;
using RegForm.Models;

namespace RegForm.Services
{
    public class RegService : IRegistration
    {
        private readonly IDBInsert _dbinsert;
        private readonly IEmail _email;
        public RegService(IDBInsert dbinsert, IEmail email)
        {
            _dbinsert = dbinsert;
            _email = email;
        }
        public ResponseModel Registration(RegFormModel regFormModel)
        {
            ResponseModel rm = _dbinsert.Registration(regFormModel);
            if (rm.Flag == 1)
            {
                EmailRequest emailRequest = new EmailRequest()
                {
                    from = ConfigManager.Appsettings["FromEmail"],
                    to = regFormModel.email,
                    subject = ConfigManager.Appsettings["MailSubject"],
                    messagebody = ConfigManager.Appsettings["MailBody"]
                };
                _email.SendEmail(emailRequest);
            }
            return rm;
        }
    }
}