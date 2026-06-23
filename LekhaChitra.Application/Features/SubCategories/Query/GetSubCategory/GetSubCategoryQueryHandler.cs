using LekhaChitra.Application.DTO.Category;
using LekhaChitra.Application.Helpers.TenantService;
using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.SubCategories.Query.GetSubCategory
{
    public class GetSubCategoryQueryHandler : IRequestHandler<GetSubCategoryQuery, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITenantService _tenantService;
        public GetSubCategoryQueryHandler(
            IUnitOfWork uow,
            ITenantService tenantService)
        {
            _uow = uow;
            _tenantService = tenantService;
        }
        public async Task<ServiceResponse> Handle(GetSubCategoryQuery request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantService.GetTenantId;
            if (tenantId == Guid.Empty)
            {
                return ServiceResponse<object>.Unauthorized("Access denied.");
            }
            var subCategories = await _uow.AsyncRepositories<SubCategory>()   
                                            .GetQueryable()
                                            .AsNoTracking()
                                            .Where(x => x.TenantId == tenantId)
                                            .OrderBy(x => x.SubCategoryName)
                                            .Select(x => new GetSubCategoryDTO
                                            {
                                                Category = x.Category.Name,
                                                SubCategory = x.SubCategoryName
                                            })
                                            .ToListAsync(cancellationToken);
            if (subCategories == null || !subCategories.Any())
            {
                return ServiceResponse.NotFound("No sub-categories found.");
            }
            else 
            {
                return ServiceResponse<object>.Success(subCategories, "Sub-categories retrieved successfully.");
            }

        }
    }
}
