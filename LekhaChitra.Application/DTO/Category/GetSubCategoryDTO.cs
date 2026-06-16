using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DTO.Category
{
    public class GetSubCategoryDTO
    {
        public string Category { get; set; }
        public string SubCategory { get; set; } = string.Empty;
    }
}

