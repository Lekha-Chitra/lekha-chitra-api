using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DTO.Transactions
{
    public record GetTransactionDTO
    {
        public Guid TransactionId { get; init; }
        public string TransactionType { get; init; }
       public string PaymentType { get; init; }
        public string To {  get; set; }
        public string From { get; set; }
        public string Client {  get; set; }

        public Guid ToId { get; set; }
        public Guid FromId { get; set; }
        public Guid ClientId { get; set; }
        public decimal Amount { get; init; }
        public string PaymentMethod { get; init; }
        public string Remarks { get; init; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? AddedDate { get; set; }

    }
}
