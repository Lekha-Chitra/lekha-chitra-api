using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DTO.Clients
{
    public record GetClientDTO
    {
        public string Name { get; init; }
        public string Status { get; init; }
        public decimal Balance { get; init; }
        public string Description { get; init; }
        public string Remarks { get; init; }
        public decimal EstimatedBudget { get; init; }
        public int Quantity { get; init; }
        public string Category { get; init; }
        public string SubCategory { get; init; }
        public DateTime? AddedDate { get; init; }
        public DateTime? ModifiedDate { get; init; }
        public DateTime? DeletedDate { get; init; }
    }
}
