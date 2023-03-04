using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegForm.Interfaces;
using RegForm.Models;

namespace RegForm.Controllers
{
    [Route("api/Register")]
    [ApiController]
    public class UserRegController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IRegistration _registration;

        public UserRegController(IConfiguration config, IRegistration registration)
        {
            _config = config;
            ConfigManager.Appsettings = _config;
            _registration = registration;
        }

        [HttpPost]
        public IActionResult Register(RegFormModel regFormModel)
        {
            var response = _registration.Registration(regFormModel);
            return Ok(response);

        }

        [HttpGet]
        public IActionResult TestException(string Test)
        {
            throw new OutOfMemoryException();
        }
    }
}
