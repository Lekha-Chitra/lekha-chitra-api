using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DTO.Transactions
{
    public class FilterTransactionRequestDTO
    {
        public record GetTransactionFilterRequest
        {
            public string? TransactionType { get; init; }
            public string? PaymentType { get; init; }
            public string? PaymentMethod { get; init; }

            public Guid? ClientId { get; init; }
            public Guid? ToId { get; init; }
            public Guid? FromId { get; init; }

            public decimal? MinAmount { get; init; }
            public decimal? MaxAmount { get; init; }

            public DateTime? FromDate { get; init; }
            public DateTime? ToDate { get; init; }

            public string? Search { get; init; }
        }
    }
}
