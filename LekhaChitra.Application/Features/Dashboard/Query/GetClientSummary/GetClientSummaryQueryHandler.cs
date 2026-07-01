using LekhaChitra.Application.Constants.Enums;
using LekhaChitra.Application.DTO.Dashboard;
using LekhaChitra.Application.Features.Dashboard.Query.GetSummary;
using LekhaChitra.Application.Helpers.Extensions;
using LekhaChitra.Application.Helpers.TenantService;
using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.Clients;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Dashboard.Query.GetClientSummary
{
    public class GetClientSummaryQueryHandler : IRequestHandler<GetClientSummaryQuery, ServiceResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ITenantService _tenantService;
        public GetClientSummaryQueryHandler(
                        IUnitOfWork uow,
                        ITenantService tenantService)
        {
            _uow = uow;
            _tenantService = tenantService;
        }

        public async Task<ServiceResponse> Handle(GetClientSummaryQuery request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantService.GetTenantId;
            if (tenantId == Guid.Empty)
            {
                return ServiceResponse<object>.Unauthorized("Access denied.");
            }
            var clients = _uow.AsyncRepositories<Client>()
                                                .GetQueryable()
                                                .AsNoTracking()
                                                .Where(x => x.TenantId == tenantId)
                                                .ApplyDateFilter(request.FromDate, request.ToDate);
            if (!clients.Any())
            {
                return ServiceResponse.NotFound("No client data found!!");
            }
            var totalClient = await clients.CountAsync(cancellationToken);
            var totalCreditClient = await clients.Where(x => x.Status == ClientStatusEnums.Credit.ToString())
                                                .CountAsync(cancellationToken);
            var totalDebitClient = await clients.Where(x => x.Status == ClientStatusEnums.Debit.ToString())
                                                .CountAsync(cancellationToken);

            var totalBalance = await clients.SumAsync(x => x.Balance, cancellationToken);
            var highestBalance = await clients.MaxAsync(x => x.Balance,cancellationToken);
            var lowestBalance = await clients.MinAsync(x => x.Balance, cancellationToken);
            var averageBalance = await clients.AverageAsync(x => x.Balance, cancellationToken);

            var clientGrowth = await clients.Where(x => x.AddedDate.HasValue)
                                            .GroupBy(x => new
                                            {
                                                x.AddedDate.Value.Year,
                                                x.AddedDate.Value.Month
                                            })
                                            .Select(x => new ClientGrowthDTO
                                            {
                                                Year = x.Key.Year,
                                                Month = x.Key.Month,
                                                MonthName = CultureInfo.CurrentCulture.DateTimeFormat
                                                                        .GetAbbreviatedMonthName(x.Key.Month),
                                                TotalClients = x.Count()
                                            })
                                           .OrderBy(x => x.Year)
                                           .ThenBy(x => x.Month)
                                           .ToListAsync(cancellationToken);

            var response = new GetClientSummaryDTO()
            { 
                TotalClients = totalClient,
                TotalCreditClients = totalCreditClient,
                TotalDebitClients = totalDebitClient,
                TotalBalance = totalBalance,
                HighestBalance = highestBalance,
                AverageBalance = averageBalance,
                LowestBalance = lowestBalance,
                ClientGrowth = clientGrowth,
            };
            return ServiceResponse<object>.Success(response);
        }
    }
}
