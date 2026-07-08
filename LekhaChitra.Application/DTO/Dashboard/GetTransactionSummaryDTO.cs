using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DTO.Dashboard
{
    public class GetTransactionSummaryDTO
    {
        public int TotalTransactions { get; set; }

        public decimal TotalTransactionAmount { get; set; }

        public decimal AverageTransactionAmount { get; set; }

        public decimal HighestTransactionAmount { get; set; }

        public decimal LowestTransactionAmount { get; set; }
        public int TotalCreditTransactions { get; set; }

        public int TotalDebitTransactions { get; set; }

        public decimal TotalCreditAmount { get; set; }

        public decimal TotalDebitAmount { get; set; }

        public List<TransactionGrowthDTO> TransactionGrowth { get; set; } = new();

        public List<TransactionTypeSummaryDTO> TransactionTypes { get; set; } = new();
        public List<PaymentMethodSummaryDTO> PaymentMethods { get; set; } = new();
    }
}
