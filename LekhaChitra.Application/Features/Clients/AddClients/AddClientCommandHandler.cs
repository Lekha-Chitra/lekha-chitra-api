using LekhaChitra.Application.Constants.Enums;
using LekhaChitra.Application.Helpers.TenantService;
using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.Categories;
using LekhaChitra.Domain.Entities.Application.Clients;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Clients.AddClients
{
    public class AddClientCommandHandler : IRequestHandler<AddClientCommand, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITenantService _tenantService;
        public AddClientCommandHandler(
            IUnitOfWork uow,
            ITenantService tenantService)
        {
            _uow = uow;
            _tenantService = tenantService;
        }
        public async Task<ServiceResponse> Handle(AddClientCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantService.GetTenantId;
            if (tenantId == Guid.Empty)
            {
                return ServiceResponse.Unauthorized("Access denied.");
            }
            var clientExists = await _uow.AsyncRepositories<Client>()
                                         .GetQueryable()
                                         .AsNoTracking()
                                         .Where(x => x.Name == request.Client.Name
                                                    && x.TenantId == tenantId)
                                         .FirstOrDefaultAsync();
            if (clientExists != null)
            {
                return ServiceResponse.Conflict("Client already exists.");
            }
            else
            {
                var subCategory = await _uow.AsyncRepositories<SubCategory>()
                                             .GetQueryable()
                                             .AsNoTracking()
                                             .Where(x => x.SubCategoryName == request.Client.SubCategory
                                                        && x.TenantId == tenantId)
                                             .FirstOrDefaultAsync();
                if (subCategory == null)
                {
                    return ServiceResponse.NotFound("SubCategory not found.");
                }
                var isStatusValid = Enum.TryParse(request.Client.Status, true, out ClientStatusEnums status);
                if (!isStatusValid)
                {
                    return ServiceResponse.BadRequest("Invalid status value.");
                }
               
                var client = new Client
                {
                    Id = Guid.NewGuid(),
                    Name = request.Client.Name,
                    Status = status.ToString(),
                    Balance = request.Client.Amount,
                    Description = request.Client.Description,
                    Remarks = request.Client.Remarks,
                    EstimatedBudget = request.Client.EstimatedBudget,
                    Quantity = request.Client.Quantity,
                    SubCategoryId = subCategory.Id,
                    TenantId = tenantId
                };
                var result = await _uow.AsyncRepositories<Client>().AddAsync(client);
                if (result == null)
                {
                    return ServiceResponse.InternalServerError("Failed to add client.");
                }
                else
                {
                    await _uow.Save();
                    return ServiceResponse.Success("Client added successfully.");
                }
            }
        }
    }
}
