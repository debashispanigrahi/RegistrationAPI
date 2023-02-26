using System;
namespace RegForm.Models
{
    public class EmailRequest
    {
        public string from { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string messagebody { get; set; }
    }
}

