using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DTO.Dashboard
{
    public class GetDashboardSummaryDTO
    {
        public int TotalClients { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal DebitAmount { get; set; }
      
    }
}
