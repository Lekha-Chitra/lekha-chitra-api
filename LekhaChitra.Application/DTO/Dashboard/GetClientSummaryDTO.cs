using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DTO.Dashboard
{
    public class GetClientSummaryDTO
    {
        public int TotalClients { get; set; }

        public int TotalCreditClients { get; set; }

        public int TotalDebitClients { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal HighestBalance { get; set; }

        public decimal LowestBalance { get; set; }

        public decimal AverageBalance { get; set; }

        public List<ClientGrowthDTO> ClientGrowth { get; set; } = new();
    }
}
