using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DTO.Transactions
{
    public class AddTransactionDTO
    {
        public string TransactionType { get; set; }
        public string PaymentType { get; set; }
        public string TO { get; set; }
        public string From { get; set; }
        public string ClientName { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Remarks { get; set; }

    }
}
