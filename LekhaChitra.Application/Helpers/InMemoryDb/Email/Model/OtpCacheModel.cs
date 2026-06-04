using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Helpers.InMemoryDb.Email.Model
{
    public class OtpCacheModel
    {
        public string Otp { get; set; }
        public DateTime ExpiryTime { get; set; }
        public int Attempts { get; set; }
        public int ResendCount { get; set; }
        public DateTime LastSentTime { get; set; }
        public DateTime? BlockedUntil { get; set; }

    }
}
