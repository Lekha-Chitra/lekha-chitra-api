using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DTO.Clients
{
    public class GetClientFilter
    {
        public string? Search { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; }
        public string? Status { get; set; }

        public decimal? MinBalance { get; set; }
        public decimal? MaxBalance { get; set; }

        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }

        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string SortBy { get; set; } = "AddedDate";
        public bool IsDescending { get; set; } = true;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
