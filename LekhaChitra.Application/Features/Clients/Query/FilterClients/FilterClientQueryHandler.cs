using LekhaChitra.Application.DTO.Clients;
using LekhaChitra.Application.Helpers.TenantService;
using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.Clients;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Clients.Query.FilterClients
{
    public class FilterClientQueryHandler : IRequestHandler<FilterClientQuery, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITenantService _tenantService;

        public FilterClientQueryHandler(
                    IUnitOfWork uow,
                    ITenantService tenantService)
        {
            _uow = uow;
            _tenantService = tenantService;
        }

        public async Task<ServiceResponse> Handle(FilterClientQuery request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantService.GetTenantId;

            if (tenantId == Guid.Empty)
                return ServiceResponse<object>.Unauthorized("Access denied.");

            var filter = request.Filter;

            var pageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize < 1 ? 10 : filter.PageSize;
            pageSize = pageSize > 100 ? 100 : pageSize;

            var query = _uow.AsyncRepositories<Client>()
                            .GetQueryable()
                            .AsNoTracking()
                            .Where(x => x.TenantId == tenantId);

            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(x => x.Name.Contains(filter.Search) ||
                                         x.Description.Contains(filter.Search) ||
                                         x.Remarks.Contains(filter.Search));

            if (!string.IsNullOrWhiteSpace(filter.Category))
            {
                query = query.Where(x => x.SubCategory.Category.Name == filter.Category);
            }
            if (!string.IsNullOrWhiteSpace(filter.SubCategory))
            {
                query = query.Where(x => x.SubCategory.SubCategoryName == filter.SubCategory);
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(x => x.Status == filter.Status);
            }

            if (filter.MinBalance.HasValue)
            {
                query = query.Where(x => x.Balance >= filter.MinBalance);
            }

            if (filter.MaxBalance.HasValue)
            {
                query = query.Where(x => x.Balance <= filter.MaxBalance);
            }

            if (filter.MinBudget.HasValue)
            {
                query = query.Where(x => x.EstimatedBudget >= filter.MinBudget);
            }
            if (filter.MaxBudget.HasValue)
            {
                query = query.Where(x => x.EstimatedBudget <= filter.MaxBudget);
            }

            if (filter.MinQuantity.HasValue)
            {
                query = query.Where(x => x.Quantity >= filter.MinQuantity);
            }

            if (filter.MaxQuantity.HasValue)
            {
                query = query.Where(x => x.Quantity <= filter.MaxQuantity);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(x => x.AddedDate >= filter.FromDate);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(x => x.AddedDate <= filter.ToDate);
            }
            query = filter.SortBy?.ToLower() switch
            {
                "name" => filter.IsDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "balance" => filter.IsDescending ? query.OrderByDescending(x => x.Balance) : query.OrderBy(x => x.Balance),
                "budget" => filter.IsDescending ? query.OrderByDescending(x => x.EstimatedBudget) : query.OrderBy(x => x.EstimatedBudget),
                _ => filter.IsDescending ? query.OrderByDescending(x => x.AddedDate) : query.OrderBy(x => x.AddedDate)
            };

            var totalRecords = await query.CountAsync(cancellationToken);

            var clients = await query
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .Select(x => new GetClientDTO
                                {
                                    Name = x.Name,
                                    Status = x.Status,
                                    Balance = x.Balance,
                                    EstimatedBudget = x.EstimatedBudget,
                                    Description = x.Description,
                                    Remarks = x.Remarks,
                                    Quantity = x.Quantity,
                                    Category = x.SubCategory.Category.Name,
                                    SubCategory = x.SubCategory.SubCategoryName,
                                    AddedDate = x.AddedDate,
                                    ModifiedDate = x.ModifiedDate,
                                    DeletedDate = x.DeletedDate
                                })
                                .ToListAsync(cancellationToken);

            return ServiceResponse<object>.Success(new
            {
                Clients = clients,
                Pagination = new
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords,
                    TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
                }
            });
        }
    }
}
