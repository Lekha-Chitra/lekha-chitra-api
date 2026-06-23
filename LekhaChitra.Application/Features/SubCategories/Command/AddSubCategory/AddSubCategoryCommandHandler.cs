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

namespace LekhaChitra.Application.Features.SubCategories.Command.AddSubCategory
{
    public class AddSubCategoryCommandHandler : IRequestHandler<AddSubCategoryCommand, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITenantService _tenantService;
        public AddSubCategoryCommandHandler(
                   IUnitOfWork uow,
                   ITenantService tenantService)
        {
            _uow = uow;
            _tenantService = tenantService;
        }
        public async Task<ServiceResponse> Handle(AddSubCategoryCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantService.GetTenantId;
            if (tenantId == Guid.Empty)
            {
                return ServiceResponse.Unauthorized("Access denied.");
            }
            var categoryExists = await _uow.AsyncRepositories<Category>()
                                        .GetQueryable()
                                        .AsNoTracking()
                                        .Where(c => c.Name == request.Category
                                                && c.TenantId == tenantId)
                                        .FirstOrDefaultAsync(cancellationToken);
            if (categoryExists is null)
            { 
                return ServiceResponse.NotFound("Enter a valid category.");
            }
            var subCategoryExists = await _uow.AsyncRepositories<SubCategory>()
                                        .GetQueryable()
                                        .AsNoTracking()
                                        .Where(x => x.SubCategoryName == request.SubCategoryName
                                                && x.CategoryId == categoryExists.Id
                                                && x.TenantId == tenantId)
                                        .FirstOrDefaultAsync(cancellationToken);
            if (subCategoryExists is not null)
            {
                return ServiceResponse.Conflict("Subcategory already exists.");
            }
            var subCategory = new SubCategory()
            {
                Id = Guid.NewGuid(),
                SubCategoryName = request.SubCategoryName,
                CategoryId = categoryExists.Id,
                TenantId = tenantId
            };
            var result = await _uow.AsyncRepositories<SubCategory>().AddAsync(subCategory);
            if (result is null)
            {
                return ServiceResponse.InternalServerError("Failed to add subcategory.");
            }
            else
            { 
                await _uow.Save();
                return ServiceResponse.Success("Subcategory added successfully.");
            }
        }
    }
}
