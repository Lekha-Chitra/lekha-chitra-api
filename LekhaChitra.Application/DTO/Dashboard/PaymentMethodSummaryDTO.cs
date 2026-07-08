using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DTO.Dashboard
{
    public class PaymentMethodSummaryDTO
    {
        public string PaymentMethod { get; set; } = string.Empty;

        public int TotalTransactions { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
