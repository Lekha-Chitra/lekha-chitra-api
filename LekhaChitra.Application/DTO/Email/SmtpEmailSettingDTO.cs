using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DTO.Email
{
    public class SmtpEmailSettingDTO
    {
        public string Provider { get; set; } = "Smtp";
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string User { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string From { get; set; } = null!;
        public bool EnableSsl { get; set; }
    }
}
