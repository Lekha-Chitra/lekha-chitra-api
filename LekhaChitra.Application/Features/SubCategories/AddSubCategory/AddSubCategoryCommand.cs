using LekhaChitra.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.SubCategories.AddSubCategory
{
    public record AddSubCategoryCommand(string Category, string SubCategoryName) : IRequest<ServiceResponse>;

}
