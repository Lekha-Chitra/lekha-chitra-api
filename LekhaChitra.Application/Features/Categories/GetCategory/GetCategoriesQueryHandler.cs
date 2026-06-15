using LekhaChitra.Application.DTO.Category;
using LekhaChitra.Application.Helpers.TenantService;
using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.Categories;
using MediatR;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Categories.GetCategory
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITenantService _tenantService;

        public GetCategoriesQueryHandler(
            IUnitOfWork uow,
            ITenantService tenantService)
        {
            _uow = uow;
            _tenantService = tenantService;
        }
        public async Task<ServiceResponse> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantService.GetTenantId;
            if (tenantId == Guid.Empty)
            {
                return ServiceResponse<object>.Unauthorized("Access denied.");
            }
            var categories = await _uow.AsyncRepositories<Category>()
                                        .GetQueryable()
                                        .AsNoTracking()
                                        .Where(c => c.TenantId == tenantId)
                                        .Select(c => new
                                        { 
                                            c.Name,
                                        })
                                        .ToListAsync(cancellationToken);

            if (categories == null || !categories.Any())
            {
                return ServiceResponse.NotFound("No categories found.");
            }
            else {
                var response = new GetCategoryDTO();
                response.Categories = new List<string>();
                foreach (var category in categories)
                {
                   response.Categories.Add(category.Name);
                }
                return ServiceResponse<object>.Success(response,"Categories retrieved successfully.");
            }
        }
    }
}
