using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DTO.Dashboard
{
    public class ClientGrowthDTO
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public string MonthName { get; set; }

        public int TotalClients { get; set; }
    }
}
