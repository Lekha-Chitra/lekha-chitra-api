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

namespace LekhaChitra.Application.Features.Clients.GetClients.GetAllClients
{
    public class GetAllClientQueryHandler : IRequestHandler<GetAllClientQuery, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITenantService _tenantService;

        public GetAllClientQueryHandler(
                    IUnitOfWork uow,
                    ITenantService tenantService)
        {
            _uow = uow;
            _tenantService = tenantService;
        }
        public async Task<ServiceResponse> Handle(GetAllClientQuery request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantService.GetTenantId;
            if (tenantId == Guid.Empty)
            {
                return ServiceResponse<object>.Unauthorized("Access denied.");
            }
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;
            pageSize = pageSize > 100 ? 100 : pageSize;

            var query = _uow.AsyncRepositories<Client>()
                            .GetQueryable()
                            .AsNoTracking()
                            .Where(x => x.TenantId == tenantId);
            var totalRecords = await query.CountAsync(cancellationToken);

            var clients = await query
                                 .OrderByDescending(x => x.AddedDate)
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
                                     DeletedDate = x.DeletedDate,
                                 })
                                .ToListAsync(cancellationToken);

            if (clients is null)
            {
                return ServiceResponse.NotFound();
            }
            else
            {
                return ServiceResponse<object>.Success(new
                {
                    Clients = clients,
                    Pagination = new
                    {
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalRecords = totalRecords,
                        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                        HasNext = pageNumber * pageSize < totalRecords,
                        HasPrevious = pageNumber > 1
                    }
                });
            }

        }
    }
}
