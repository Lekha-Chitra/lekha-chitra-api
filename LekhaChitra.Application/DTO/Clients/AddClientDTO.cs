using LekhaChitra.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DTO.Clients
{
    public class AddClientDTO
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public decimal EstimatedBudget { get; set; }
        public int Quantity { get; set; }
        public string SubCategory { get; set; }
    }


}
